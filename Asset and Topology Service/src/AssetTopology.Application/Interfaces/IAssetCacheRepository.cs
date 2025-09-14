using System;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Application.Models;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that manages the caching of asset hierarchy data.
    /// This provides a high-performance access layer for frequently read, slow-changing hierarchical data.
    /// </summary>
    public interface IAssetCacheRepository
    {
        /// <summary>
        /// Asynchronously retrieves the cached asset hierarchy for a specific tenant.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the cached asset hierarchy DTO if found; otherwise, null.
        /// </returns>
        Task<AssetNodeDto?> GetHierarchyAsync(Guid tenantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously stores the asset hierarchy for a specific tenant in the cache.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <param name="hierarchy">The asset hierarchy DTO to cache.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetHierarchyAsync(Guid tenantId, AssetNodeDto hierarchy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously invalidates and removes the cached asset hierarchy for a specific tenant.
        /// This should be called after any CUD operation on the asset structure to prevent stale data.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant whose cache should be invalidated.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InvalidateHierarchyAsync(Guid tenantId, CancellationToken cancellationToken = default);
    }
}