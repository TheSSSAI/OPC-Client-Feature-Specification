using System;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Application.Models;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for the application service that handles asset management use cases.
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        /// Asynchronously retrieves the full, nested asset hierarchy for a given tenant.
        /// This operation is expected to be high-performance, utilizing a cache.
        /// </summary>
        /// <param name="tenantId">The unique identifier for the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO representing the root of the asset hierarchy, or null if no assets exist.</returns>
        Task<AssetNodeDto?> GetAssetHierarchyAsync(Guid tenantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves the details of a single asset.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="assetId">The unique identifier of the asset.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO with the asset's details, or null if not found.</returns>
        Task<AssetDetailDto?> GetAssetByIdAsync(Guid tenantId, Guid assetId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new asset.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="request">The request DTO containing information for the new asset.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO representing the newly created asset.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.ValidationException">Thrown when request validation fails.</exception>
        /// <exception cref="AssetTopology.Application.Exceptions.ParentAssetNotFoundException">Thrown if the specified parent asset does not exist.</exception>
        Task<AssetDetailDto> CreateAssetAsync(Guid tenantId, CreateAssetRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an existing asset's details.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="assetId">The unique identifier of the asset to update.</param>
        /// <param name="request">The request DTO containing the updated information.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetNotFoundException">Thrown if the asset to update is not found.</exception>
        Task UpdateAssetAsync(Guid tenantId, Guid assetId, UpdateAssetRequest request, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Asynchronously moves an existing asset to a new parent.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="assetId">The unique identifier of the asset to move.</param>
        /// <param name="newParentId">The unique identifier of the new parent asset. Can be null to move to root.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetNotFoundException">Thrown if the asset or new parent is not found.</exception>
        /// <exception cref="AssetTopology.Application.Exceptions.CircularDependencyException">Thrown if the move would create a circular dependency.</exception>
        Task MoveAssetAsync(Guid tenantId, Guid assetId, Guid? newParentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes an asset and all of its descendants.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="assetId">The unique identifier of the asset to delete.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetNotFoundException">Thrown if the asset to delete is not found.</exception>
        Task DeleteAssetAsync(Guid tenantId, Guid assetId, CancellationToken cancellationToken = default);
    }
}