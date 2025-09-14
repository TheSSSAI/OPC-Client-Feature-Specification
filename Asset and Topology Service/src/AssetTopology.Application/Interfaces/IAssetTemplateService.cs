using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Application.Models;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for the application service that handles asset template management use cases.
    /// </summary>
    public interface IAssetTemplateService
    {
        /// <summary>
        /// Asynchronously creates a new asset template.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="request">The DTO containing information for the new template.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO representing the newly created asset template.</returns>
        Task<AssetTemplateDto> CreateTemplateAsync(Guid tenantId, CreateAssetTemplateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an asset template by its ID.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="templateId">The unique identifier of the template.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO of the asset template, or null if not found.</returns>
        Task<AssetTemplateDto?> GetTemplateByIdAsync(Guid tenantId, Guid templateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves all asset templates for a tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A collection of asset template DTOs.</returns>
        Task<IEnumerable<AssetTemplateDto>> GetAllTemplatesAsync(Guid tenantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an existing asset template.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="templateId">The unique identifier of the template to update.</param>
        /// <param name="request">The DTO containing the updated template information.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetTemplateNotFoundException">Thrown if the template to update is not found.</exception>
        Task UpdateTemplateAsync(Guid tenantId, Guid templateId, UpdateAssetTemplateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes an asset template.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="templateId">The unique identifier of the template to delete.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetTemplateNotFoundException">Thrown if the template to delete is not found.</exception>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetTemplateInUseException">Thrown if the template is currently used by assets.</exception>
        Task DeleteTemplateAsync(Guid tenantId, Guid templateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new asset instance from a template.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="request">The DTO containing information for instantiating the asset from a template.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A DTO of the newly created root asset from the template instance.</returns>
        /// <exception cref="AssetTopology.Application.Exceptions.AssetTemplateNotFoundException">Thrown if the specified template is not found.</exception>
        /// <exception cref="AssetTopology.Application.Exceptions.ParentAssetNotFoundException">Thrown if the specified parent asset for instantiation is not found.</exception>
        Task<AssetDetailDto> CreateAssetFromTemplateAsync(Guid tenantId, InstantiateAssetFromTemplateRequest request, CancellationToken cancellationToken = default);
    }
}