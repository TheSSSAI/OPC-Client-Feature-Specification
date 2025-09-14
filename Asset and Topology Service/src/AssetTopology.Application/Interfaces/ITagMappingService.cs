using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Application.Models;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for the application service that handles the mapping of OPC tags to assets.
    /// </summary>
    public interface ITagMappingService
    {
        /// <summary>
        /// Asynchronously retrieves all OPC tag mappings for a specific asset.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="assetId">The unique identifier of the asset.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A collection of DTOs representing the tag mappings for the asset.</returns>
        Task<IEnumerable<OpcTagDto>> GetMappingsForAssetAsync(Guid tenantId, Guid assetId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new mapping between an OPC tag and an asset.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="assetId">The unique identifier of the asset to map the tag to.</param>
        /// <param name="request">The request DTO containing the tag information.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO representing the newly created tag mapping.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetNotFoundException">Thrown if the specified asset does not exist.</exception>
        /// <exception cref="AssetTopology.Application.Exceptions.TagAlreadyMappedException">Thrown if the OPC tag is already mapped to another asset.</exception>
        Task<OpcTagDto> CreateMappingAsync(Guid tenantId, Guid assetId, CreateOpcTagMappingRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes an existing OPC tag mapping.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="tagId">The unique identifier of the OPC tag mapping to delete.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.OpcTagNotFoundException">Thrown if the tag mapping does not exist.</exception>
        Task DeleteMappingAsync(Guid tenantId, Guid tagId, CancellationToken cancellationToken = default);
    }
}