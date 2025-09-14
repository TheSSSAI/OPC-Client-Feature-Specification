using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System.Services.DeviceManagement.Application.Interfaces;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Services.Shared.Infrastructure.Mqtt; // Assuming a shared wrapper and DTOs

namespace System.Services.DeviceManagement.Infrastructure.Mqtt
{
    /// <summary>
    /// Implements the contract for publishing command messages to the MQTT broker.
    /// </summary>
    public class MqttCommandPublisher : IMqttCommandPublisher
    {
        private readonly MqttClientService _mqttClientService;
        private readonly ILogger<MqttCommandPublisher> _logger;

        public MqttCommandPublisher(MqttClientService mqttClientService, ILogger<MqttCommandPublisher> logger)
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task PublishConfigurationUpdateAsync(Guid tenantId, Guid clientId, string newConfigurationJson, CancellationToken cancellationToken = default)
        {
            var command = new MqttCommand<ConfigurationUpdatePayload>
            {
                CommandType = MqttCommandType.UpdateConfiguration,
                Payload = new ConfigurationUpdatePayload { ConfigurationJson = newConfigurationJson }
            };
            
            await PublishCommandAsync(tenantId, clientId, command, cancellationToken);
        }

        /// <inheritdoc />
        public async Task PublishSoftwareUpdateAsync(Guid tenantId, Guid clientId, string imageUrl, string checksum, CancellationToken cancellationToken = default)
        {
             var command = new MqttCommand<SoftwareUpdatePayload>
            {
                CommandType = MqttCommandType.UpdateSoftware,
                Payload = new SoftwareUpdatePayload { ImageUrl = imageUrl, Checksum = checksum }
            };
            
            await PublishCommandAsync(tenantId, clientId, command, cancellationToken);
        }
        
        /// <inheritdoc />
        public async Task PublishSoftwareRollbackAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken = default)
        {
            var command = new MqttCommand<object> // No payload needed for rollback
            {
                CommandType = MqttCommandType.RollbackSoftware,
                Payload = new {}
            };
            
            await PublishCommandAsync(tenantId, clientId, command, cancellationToken);
        }

        private async Task PublishCommandAsync<T>(Guid tenantId, Guid clientId, MqttCommand<T> command, CancellationToken cancellationToken)
        {
            var mqttClient = _mqttClientService.GetMqttClient();
            if (mqttClient == null || !mqttClient.IsConnected)
            {
                _logger.LogError("Cannot publish command {CommandType} to client {ClientId}. MQTT client is not connected.", command.CommandType, clientId);
                // In a real system, this might throw an exception or queue the command for later.
                // For now, we log and fail silently to avoid blocking the API.
                return;
            }

            var topic = MqttTopicHelper.GetClientCommandTopic(tenantId, clientId);
            var payload = JsonSerializer.Serialize(command);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce) // QoS 1 for reliable delivery
                .WithRetainFlag(false)
                .Build();

            try
            {
                var result = await mqttClient.PublishAsync(message, cancellationToken);
                if (result.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    _logger.LogInformation("Successfully published command {CommandType} to topic {Topic}", command.CommandType, topic);
                }
                else
                {
                    _logger.LogError("Failed to publish command {CommandType} to topic {Topic}. Reason: {ReasonCode}", command.CommandType, topic, result.ReasonCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while publishing command {CommandType} to topic {Topic}", command.CommandType, topic);
                throw; // Re-throw to let the caller know the publish failed
            }
        }
    }
}