using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using System.Text.RegularExpressions;
using DeviceManagement.Infrastructure.Configuration;

namespace DeviceManagement.Infrastructure.Mqtt;

/// <summary>
/// Manages the persistent connection to the MQTT broker, subscribes to client status topics,
/// and dispatches incoming messages for processing. This is a long-running background service.
/// Fulfills requirement REQ-1-010 for MQTT-based status updates.
/// </summary>
public class MqttClientService : BackgroundService
{
    private readonly ILogger<MqttClientService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly MqttSettings _mqttSettings;
    private readonly IMqttClient _mqttClient;
    private static readonly Regex StatusTopicRegex = new(@"^tenants/([0-9a-fA-F\-]{36})/clients/([0-9a-fA-F\-]{36})/status$", RegexOptions.Compiled);

    public MqttClientService(
        ILogger<MqttClientService> logger,
        IServiceProvider serviceProvider,
        IOptions<MqttSettings> mqttSettingsOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mqttSettings = mqttSettingsOptions.Value;

        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
        _mqttClient.DisconnectedAsync += async e =>
        {
            _logger.LogWarning("Disconnected from MQTT broker. Reason: {Reason}. Will attempt to reconnect...", e.ReasonString);
            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                await ConnectToBrokerAsync(stoppingToken);
            }
        };

        await ConnectToBrokerAsync(stoppingToken);

        // Keep the service running
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ConnectToBrokerAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;

        try
        {
            if (_mqttClient.IsConnected) return;

            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_mqttSettings.BrokerAddress, _mqttSettings.Port)
                .WithCredentials(_mqttSettings.Username, _mqttSettings.Password)
                .WithClientId($"DeviceManagementService-{Guid.NewGuid()}")
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithCleanSession()
                .WithTls(new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    AllowUntrustedCertificates = true // Note: Set to false in production with a proper CA
                })
                .Build();

            _logger.LogInformation("Connecting to MQTT broker at {Address}:{Port}...", _mqttSettings.BrokerAddress, _mqttSettings.Port);
            var result = await _mqttClient.ConnectAsync(mqttClientOptions, stoppingToken);

            if (result.ResultCode == MqttClientConnectResultCode.Success)
            {
                _logger.LogInformation("Successfully connected to MQTT broker.");
                await SubscribeToTopicsAsync(stoppingToken);
            }
            else
            {
                _logger.LogError("Failed to connect to MQTT broker. Result code: {ResultCode}", result.ResultCode);
            }
        }
        catch (MqttCommunicationException ex)
        {
            _logger.LogError(ex, "Communication error while connecting to MQTT broker.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while connecting to MQTT broker.");
        }
    }

    private async Task SubscribeToTopicsAsync(CancellationToken stoppingToken)
    {
        // This wildcard subscription receives status updates from all clients across all tenants.
        // For very large-scale deployments, consider MQTTv5 Shared Subscriptions to load-balance
        // messages across multiple instances of this service.
        var topicFilter = new MqttTopicFilterBuilder()
            .WithTopic("tenants/+/clients/+/status")
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        try
        {
            var subscribeResult = await _mqttClient.SubscribeAsync(topicFilter, stoppingToken);
            foreach (var subscription in subscribeResult.Items)
            {
                if (subscription.ResultCode == MqttClientSubscribeResultCode.GrantedQoS0 ||
                    subscription.ResultCode == MqttClientSubscribeResultCode.GrantedQoS1 ||
                    subscription.ResultCode == MqttClientSubscribeResultCode.GrantedQoS2)
                {
                    _logger.LogInformation("Successfully subscribed to topic: {Topic}", subscription.TopicFilter.Topic);
                }
                else
                {
                    _logger.LogError("Failed to subscribe to topic {Topic}. Reason: {Reason}", subscription.TopicFilter.Topic, subscription.ResultCode);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while subscribing to MQTT topics.");
        }
    }

    private async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        _logger.LogDebug("Received MQTT message on topic: {Topic}", e.ApplicationMessage.Topic);

        try
        {
            var match = StatusTopicRegex.Match(e.ApplicationMessage.Topic);
            if (!match.Success)
            {
                _logger.LogWarning("Received message on an un-parsable status topic: {Topic}", e.ApplicationMessage.Topic);
                return;
            }

            if (!Guid.TryParse(match.Groups[1].Value, out var tenantId) ||
                !Guid.TryParse(match.Groups[2].Value, out var clientId))
            {
                _logger.LogError("Could not parse TenantId or ClientId from topic: {Topic}", e.ApplicationMessage.Topic);
                return;
            }

            var payload = e.ApplicationMessage.PayloadSegment.Count > 0
                ? System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)
                : string.Empty;

            if (string.IsNullOrEmpty(payload))
            {
                _logger.LogWarning("Received empty payload for topic {Topic}", e.ApplicationMessage.Topic);
                return;
            }

            // Create a new DI scope to resolve scoped services like DbContext
            // This is a critical pattern for handling messages in a singleton background service
            await using var scope = _serviceProvider.CreateAsyncScope();
            var messageHandler = scope.ServiceProvider.GetRequiredService<MqttStatusMessageHandler>();

            await messageHandler.HandleStatusMessageAsync(tenantId, clientId, payload);
        }
        catch (Exception ex)
        {
            // Catching exceptions here is crucial to prevent the background service from crashing
            // due to a single malformed or problematic message.
            _logger.LogError(ex, "Error processing MQTT message from topic {Topic}", e.ApplicationMessage.Topic);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MqttClientService is stopping.");
        _mqttClient.ApplicationMessageReceivedAsync -= HandleApplicationMessageReceivedAsync;
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptions(), cancellationToken);
        }
        await base.StopAsync(cancellationToken);
    }
}