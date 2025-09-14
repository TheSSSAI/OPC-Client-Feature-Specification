using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetTopology.Domain.Entities;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for data persistence operations related to the AssetTemplate entity.
    /// </summary>
    public interface IAssetTemplateRepository
    {
        /// <summary>
        /// Asynchronously retrieves an asset template by its unique identifier.
        /// </summary>
        /// <param name="id">The template's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The asset template if found; otherwise, null.</returns>
        Task<AssetTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves all asset templates for a given tenant.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>An enumerable collection of asset templates.</returns>
        Task<IEnumerable<AssetTemplate>> GetAllByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds a new asset template to the persistence store.
        /// </summary>
        /// <param name="template">The asset template to add.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(AssetTemplate template, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an asset template entity as modified.
        /// </summary>
        /// <param name="template">The asset template to update.</param>
        void Update(AssetTemplate template);

        /// <summary>
        /// Removes an asset template entity from the persistence store.
        /// </summary>
        /// <param name="template">The asset template to remove.</param>
        void Delete(AssetTemplate template);

        /// <summary>
        /// Asynchronously checks if a template is being used by any assets.
        /// </summary>
        /// <param name="templateId">The ID of the template to check.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>True if the template is in use; otherwise, false.</returns>
        Task<bool> IsTemplateInUseAsync(Guid templateId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Asynchronously checks if a template with the given name already exists for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="name">The name of the template to check.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>True if a template with the same name exists; otherwise, false.</returns>
        Task<bool> ExistsByNameAsync(Guid tenantId, string name, CancellationToken cancellationToken = default);
    }
}