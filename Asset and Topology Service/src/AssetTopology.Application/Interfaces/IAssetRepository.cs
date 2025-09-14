using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Domain.Entities;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for data persistence operations related to the Asset entity.
    /// </summary>
    public interface IAssetRepository
    {
        /// <summary>
        /// Asynchronously retrieves an asset by its unique identifier.
        /// </summary>
        /// <param name="id">The asset's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The asset if found; otherwise, null.</returns>
        Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Asynchronously retrieves an asset by its unique identifier, including its child assets.
        /// </summary>
        /// <param name="id">The asset's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The asset with its children if found; otherwise, null.</returns>
        Task<Asset?> GetByIdWithChildrenAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a flat list of all assets for a given tenant.
        /// The implementation should be optimized for building a hierarchy.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>An enumerable collection of all assets for the tenant.</returns>
        Task<IEnumerable<Asset>> GetHierarchyByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds a new asset to the persistence store.
        /// </summary>
        /// <param name="asset">The asset entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Asset asset, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an asset entity as modified. The actual update happens when SaveChangesAsync is called.
        /// </summary>
        /// <param name="asset">The asset entity to update.</param>
        void Update(Asset asset);

        /// <summary>
        /// Removes an asset entity from the persistence store.
        /// </summary>
        /// <param name="asset">The asset entity to remove.</param>
        void Delete(Asset asset);

        /// <summary>
        /// Asynchronously checks if an asset with the given name already exists under the specified parent.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="name">The name of the asset to check.</param>
        /// <param name="parentAssetId">The identifier of the parent asset. Null for root assets.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>True if a sibling with the same name exists; otherwise, false.</returns>
        Task<bool> ExistsByNameAsync(Guid tenantId, string name, Guid? parentAssetId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Asynchronously retrieves all descendant IDs for a given asset.
        /// </summary>
        /// <param name="assetId">The ID of the parent asset.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A list of GUIDs representing all descendant assets.</returns>
        Task<List<Guid>> GetDescendantIdsAsync(Guid assetId, CancellationToken cancellationToken = default);
    }
}