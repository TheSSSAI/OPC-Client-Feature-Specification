using DeviceManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceManagement.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that handles data access for OpcCoreClient entities.
    /// This interface is part of the Domain layer and enforces the Dependency Inversion Principle,
    /// allowing the Application layer to be independent of the data persistence technology.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Retrieves an OPC Core Client by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the client.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the client if found; otherwise, null.</returns>
        Task<OpcCoreClient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all OPC Core Clients associated with a specific tenant.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of clients for the specified tenant.</returns>
        Task<IEnumerable<OpcCoreClient>> GetAllForTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an OPC Core Client by its unique certificate common name.
        /// This is a critical method for authenticating clients connecting via MQTT with mTLS.
        /// </summary>
        /// <param name="commonName">The common name from the client's X.509 certificate.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the client if found; otherwise, null.</returns>
        Task<OpcCoreClient?> GetByCertificateCommonNameAsync(string commonName, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Adds a new OPC Core Client to the repository.
        /// </summary>
        /// <param name="client">The client entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(OpcCoreClient client, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing OPC Core Client in the repository.
        /// </summary>
        /// <param name="client">The client entity with updated values.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(OpcCoreClient client, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an OPC Core Client from the repository.
        /// </summary>
        /// <param name="client">The client entity to delete.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(OpcCoreClient client, CancellationToken cancellationToken = default);
    }
}