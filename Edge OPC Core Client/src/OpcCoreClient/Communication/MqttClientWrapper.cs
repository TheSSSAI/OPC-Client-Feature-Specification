using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Edge.OpcCoreClient.Configuration;
using System.Edge.OpcCoreClient.Services;

namespace System.Edge.OpcCoreClient.Communication
{
    /// <summary>
    /// A wrapper around the MQTTnet client that handles connection management,
    /// automatic reconnection, subscriptions, and message dispatching.
    /// This service acts as the primary control plane communication channel.
    /// </summary>
    public class MqttClientWrapper : IHostedService, IDisposable
    {
        private readonly ILogger<MqttClientWrapper> _logger;
        private readonly MqttOptions _mqttOptions;
        private readonly MqttCommandHandler _commandHandler;
        private readonly IConnectivityStateManager _connectivityStateManager;
        private IManagedMqttClient? _mqttClient;
        private readonly MqttFactory _mqttFactory;

        public MqttClientWrapper(
            ILogger<MqttClientWrapper> logger,
            IOptions<MqttOptions> mqttOptions,
            MqttCommandHandler commandHandler,
            IConnectivityStateManager connectivityStateManager,
            MqttFactory mqttFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mqttOptions = mqttOptions?.Value ?? throw new ArgumentNullException(nameof(mqttOptions));
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _connectivityStateManager = connectivityStateManager ?? throw new ArgumentNullException(nameof(connectivityStateManager));
            _mqttFactory = mqttFactory ?? throw new ArgumentNullException(nameof(mqttFactory));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MQTT Client Wrapper is starting.");

            var managedClientOptions = BuildManagedMqttClientOptions();

            _mqttClient = _mqttFactory.CreateManagedMqttClient();
            
            _mqttClient.ConnectedAsync += OnConnectedAsync;
            _mqttClient.DisconnectedAsync += OnDisconnectedAsync;
            _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;

            await _mqttClient.StartAsync(managedClientOptions);
            _logger.LogInformation("Managed MQTT client started. Awaiting connection...");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MQTT Client Wrapper is stopping.");
            if (_mqttClient != null)
            {
                _mqttClient.ConnectedAsync -= OnConnectedAsync;
                _mqttClient.DisconnectedAsync -= OnDisconnectedAsync;
                _mqttClient.ApplicationMessageReceivedAsync -= OnApplicationMessageReceivedAsync;
                
                if (_mqttClient.IsStarted)
                {
                    await _mqttClient.StopAsync(true);
                }
            }
        }

        public async Task PublishAsync(string topic, string payload, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce, bool retain = false)
        {
            if (_mqttClient == null || !_mqttClient.IsConnected)
            {
                _logger.LogWarning("Cannot publish message. MQTT client is not connected.");
                return;
            }

            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(qos)
                    .WithRetainFlag(retain)
                    .Build();

                await _mqttClient.EnqueueAsync(message);
                _logger.LogDebug("Successfully enqueued MQTT message for topic {Topic}", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing MQTT message to topic {Topic}", topic);
            }
        }

        private ManagedMqttClientOptions BuildManagedMqttClientOptions()
        {
            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = _mqttOptions.UseTls,
                AllowUntrustedCertificates = false, // Must be false in production
                // SslProtocol = SslProtocols.Tls13, // Configure as needed
                // CertificateValidationHandler = ... // For custom validation
            };

            var clientOptions = new MqttClientOptionsBuilder()
                .WithClientId(_mqttOptions.ClientId)
                .WithTcpServer(_mqttOptions.BrokerHost, _mqttOptions.BrokerPort)
                .WithTlsOptions(tlsOptions)
                .WithCleanSession(false) // For persistent sessions
                .Build();

            return new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(clientOptions)
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithMaxPendingMessages(1000)
                .Build();
        }

        private async Task OnConnectedAsync(MqttClientConnectedEventArgs e)
        {
            _logger.LogInformation("Successfully connected to MQTT broker.");
            // Here you could update a more granular state, e.g. _connectivityStateManager.SetMqttState(true);

            if (_mqttClient == null) return;
            
            var commandTopic = string.Format(_mqttOptions.CommandTopicTemplate, _mqttOptions.TenantId, _mqttOptions.ClientId);
            _logger.LogInformation("Subscribing to command topic: {Topic}", commandTopic);
            
            try
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                    .WithTopic(commandTopic)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build());
                
                _logger.LogInformation("Successfully subscribed to command topic.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe to command topic {Topic}", commandTopic);
            }
        }

        private Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs e)
        {
            _logger.LogWarning(e.Exception, "Disconnected from MQTT broker. Reason: {Reason}. Will try to reconnect automatically.", e.ReasonString);
            // Here you could update a more granular state, e.g. _connectivityStateManager.SetMqttState(false);
            return Task.CompletedTask;
        }

        private async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = e.ApplicationMessage.PayloadSegment.Count > 0 ? Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment) : string.Empty;

            _logger.LogInformation("Received MQTT message on topic '{Topic}'", topic);
            _logger.LogTrace("Payload: {Payload}", payload);

            try
            {
                // Assuming a common command structure for deserialization
                var command = JsonSerializer.Deserialize<MqttCommand>(payload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (command != null)
                {
                    await _commandHandler.HandleCommandAsync(command);
                }
                else
                {
                    _logger.LogWarning("Failed to deserialize MQTT message payload on topic {Topic} into a known command.", topic);
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON Deserialization failed for message on topic {Topic}.", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MQTT message from topic {Topic}.", topic);
            }
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }
    }

    /// <summary>
    /// A generic DTO for MQTT commands.
    /// </summary>
    public record MqttCommand(string CommandType, JsonElement Payload);
}