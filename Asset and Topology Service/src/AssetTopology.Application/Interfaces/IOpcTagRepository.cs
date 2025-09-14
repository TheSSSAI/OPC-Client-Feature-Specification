using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Domain.Entities;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for data persistence operations related to the OpcTag entity.
    /// </summary>
    public interface IOpcTagRepository
    {
        /// <summary>
        /// Asynchronously retrieves an OPC tag by its unique identifier.
        /// </summary>
        /// <param name="id">The tag's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The OPC tag if found; otherwise, null.</returns>
        Task<OpcTag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves all OPC tags mapped to a specific asset.
        /// </summary>
        /// <param name="assetId">The unique identifier of the asset.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>An enumerable collection of OPC tags mapped to the asset.</returns>
        Task<IEnumerable<OpcTag>> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds a new OPC tag to the persistence store.
        /// </summary>
        /// <param name="opcTag">The OPC tag entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(OpcTag opcTag, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an OPC tag entity from the persistence store.
        /// </summary>
        /// <param name="opcTag">The OPC tag entity to remove.</param>
        void Delete(OpcTag opcTag);

        /// <summary>
        /// Asynchronously checks if an OPC tag with the given NodeId is already mapped to any asset within the tenant.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <param name="nodeId">The OPC tag NodeId to check.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>True if the tag is already mapped; otherwise, false.</returns>
        Task<bool> IsTagMappedAsync(Guid tenantId, string nodeId, CancellationToken cancellationToken = default);
    }
}