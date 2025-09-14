using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Services.DeviceManagement.Domain.Entities;
using System.Services.DeviceManagement.Domain.Enums;
using System.Services.DeviceManagement.Domain.Interfaces;
using System.Services.Shared.Infrastructure.Mqtt; // Assuming shared DTOs

namespace System.Services.DeviceManagement.Infrastructure.Mqtt
{
    /// <summary>
    /// Processes incoming status messages from OPC Core Clients received via MQTT.
    /// </summary>
    public class MqttStatusMessageHandler
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<MqttStatusMessageHandler> _logger;

        public MqttStatusMessageHandler(IClientRepository clientRepository, ILogger<MqttStatusMessageHandler> logger)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handles a raw MQTT message, deserializes it, and updates the client's state.
        /// </summary>
        /// <param name="clientId">The client ID, authenticated from the client certificate.</param>
        /// <param name="payload">The JSON payload of the message.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        public async Task HandleMessageAsync(Guid clientId, string payload, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(payload))
            {
                _logger.LogWarning("Received an empty status payload for client {ClientId}.", clientId);
                return;
            }

            try
            {
                var statusMessage = JsonSerializer.Deserialize<MqttStatusMessage>(payload);
                if (statusMessage == null)
                {
                    _logger.LogError("Failed to deserialize status message for client {ClientId}. Payload: {Payload}", clientId, payload);
                    return;
                }

                var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
                if (client == null)
                {
                    _logger.LogWarning("Received status message from an unknown client with ID {ClientId}. Ignoring.", clientId);
                    return;
                }
                
                // Update common properties
                client.LastSeenUtc = DateTime.UtcNow;
                client.Status = statusMessage.Status;
                client.SoftwareVersion = statusMessage.Version;
                
                // Handle specific payload types
                if (statusMessage.Status == ClientStatus.Online && statusMessage.HealthMetrics != null)
                {
                    // In a full implementation, these would be persisted for timeseries analysis.
                    // For now, we might store the latest snapshot on the client entity.
                    _logger.LogDebug("Received health metrics from {ClientId}: CPU={CpuUsage}%, Memory={MemoryUsage}MB",
                        clientId, statusMessage.HealthMetrics.CpuUsagePercent, statusMessage.HealthMetrics.MemoryUsageMb);
                }
                else if (statusMessage.UpdateResult != null)
                {
                    HandleUpdateResult(client, statusMessage.UpdateResult);
                }

                await _clientRepository.UpdateAsync(client, cancellationToken);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON Deserialization failed for status message from client {ClientId}. Payload: {Payload}", clientId, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing status message for client {ClientId}.", clientId);
            }
        }
        
        private void HandleUpdateResult(OpcCoreClient client, UpdateResultPayload updateResult)
        {
            if (updateResult.Success)
            {
                client.Status = ClientStatus.Online;
                _logger.LogInformation("Client {ClientId} reported successful update to version {Version}.", client.Id, updateResult.Version);
                // The version is already updated from the main status message part
            }
            else
            {
                client.Status = ClientStatus.Error; // Or a specific 'UpdateFailed' status
                _logger.LogError("Client {ClientId} reported failed update to version {Version}. Reason: {ErrorMessage}",
                    client.Id, updateResult.Version, updateResult.ErrorMessage);
            }
        }
    }
}