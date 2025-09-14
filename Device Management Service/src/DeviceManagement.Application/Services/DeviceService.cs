using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Services.DeviceManagement.Application.Interfaces;
using System.Services.DeviceManagement.Domain.Entities;
using System.Services.DeviceManagement.Domain.Enums;
using System.Services.DeviceManagement.Domain.Interfaces;
using System.Services.Shared.Application.Exceptions;

namespace System.Services.DeviceManagement.Application.Services
{
    /// <summary>
    /// Provides application services for managing the lifecycle and configuration of OPC Core Clients.
    /// </summary>
    public class DeviceService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMqttCommandPublisher _mqttCommandPublisher;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(
            IClientRepository clientRepository,
            IMqttCommandPublisher mqttCommandPublisher,
            ILogger<DeviceService> logger)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _mqttCommandPublisher = mqttCommandPublisher ?? throw new ArgumentNullException(nameof(mqttCommandPublisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all OPC Core Clients for a specific tenant.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A collection of OPC Core Client entities.</returns>
        public async Task<IEnumerable<OpcCoreClient>> GetClientsForTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }

            return await _clientRepository.GetAllForTenantAsync(tenantId, cancellationToken);
        }

        /// <summary>
        /// Retrieves a single OPC Core Client by its ID, ensuring it belongs to the specified tenant.
        /// </summary>
        /// <param name="clientId">The ID of the client.</param>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The OPC Core Client entity if found; otherwise, null.</returns>
        public async Task<OpcCoreClient?> GetClientByIdAsync(Guid clientId, Guid tenantId, CancellationToken cancellationToken = default)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
            }
             if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }

            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);

            if (client == null || client.TenantId != tenantId)
            {
                return null;
            }

            return client;
        }

        /// <summary>
        /// Initiates a remote configuration update for a specific client.
        /// </summary>
        /// <param name="clientId">The ID of the client to update.</param>
        /// <param name="tenantId">The ID of the tenant that owns the client.</param>
        /// <param name="newConfigurationJson">The new configuration in JSON format.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateClientConfigurationAsync(Guid clientId, Guid tenantId, string newConfigurationJson, CancellationToken cancellationToken = default)
        {
            var client = await GetAndValidateClientAsync(clientId, tenantId, cancellationToken);
            
            client.ConfigurationJson = newConfigurationJson;
            await _clientRepository.UpdateAsync(client, cancellationToken);

            await _mqttCommandPublisher.PublishConfigurationUpdateAsync(tenantId, clientId, newConfigurationJson, cancellationToken);

            _logger.LogInformation("Successfully initiated configuration update for client {ClientId} in tenant {TenantId}.", clientId, tenantId);
        }

        /// <summary>
        /// Initiates a remote software update for a specific client.
        /// </summary>
        /// <param name="clientId">The ID of the client to update.</param>
        /// <param name="tenantId">The ID of the tenant that owns the client.</param>
        /// <param name="imageUrl">The URL of the new Docker image.</param>
        /// <param name="checksum">The checksum of the image for verification.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateClientSoftwareAsync(Guid clientId, Guid tenantId, string imageUrl, string checksum, CancellationToken cancellationToken = default)
        {
            var client = await GetAndValidateClientAsync(clientId, tenantId, cancellationToken);
            
            client.Status = ClientStatus.Updating;
            await _clientRepository.UpdateAsync(client, cancellationToken);

            await _mqttCommandPublisher.PublishSoftwareUpdateAsync(tenantId, clientId, imageUrl, checksum, cancellationToken);
            
            _logger.LogInformation("Successfully initiated software update to version {ImageUrl} for client {ClientId} in tenant {TenantId}.", imageUrl, clientId, tenantId);
        }
        
        /// <summary>
        /// Initiates a remote software rollback for a specific client.
        /// </summary>
        /// <param name="clientId">The ID of the client to roll back.</param>
        /// <param name="tenantId">The ID of the tenant that owns the client.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RollbackClientSoftwareAsync(Guid clientId, Guid tenantId, CancellationToken cancellationToken = default)
        {
            var client = await GetAndValidateClientAsync(clientId, tenantId, cancellationToken);

            // In a real implementation, you'd fetch the previous version from a history table or similar mechanism.
            // For now, we assume the client agent knows its previous version.
            // A more complete implementation would pass the target rollback version here.
            
            client.Status = ClientStatus.Updating; // "Updating" state is also used for rollback
            await _clientRepository.UpdateAsync(client, cancellationToken);

            await _mqttCommandPublisher.PublishSoftwareRollbackAsync(tenantId, clientId, cancellationToken);

            _logger.LogInformation("Successfully initiated software rollback for client {ClientId} in tenant {TenantId}.", clientId, tenantId);
        }

        private async Task<OpcCoreClient> GetAndValidateClientAsync(Guid clientId, Guid tenantId, CancellationToken cancellationToken)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
            }
            if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }

            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException(nameof(OpcCoreClient), clientId);
            }

            // Critical security check for tenant isolation
            if (client.TenantId != tenantId)
            {
                _logger.LogWarning("Tenant {AttemptedTenantId} tried to access client {ClientId} owned by {OwnerTenantId}. Access denied.", tenantId, clientId, client.TenantId);
                throw new ForbiddenAccessException("Access to the specified client is denied.");
            }

            return client;
        }
    }
}