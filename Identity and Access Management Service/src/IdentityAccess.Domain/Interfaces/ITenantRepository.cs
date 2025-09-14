using System.Services.IdentityAccess.Domain.Entities;

namespace System.Services.IdentityAccess.Domain.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages Tenant entities.
/// </summary>
public interface ITenantRepository
{
    /// <summary>
    /// Gets a tenant by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tenant.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The tenant if found; otherwise, null.</returns>
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all tenants.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of all tenants.</returns>
    Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new tenant to the repository.
    /// </summary>
    /// <param name="tenant">The tenant to add.</param>
    void Add(Tenant tenant);

    /// <summary>
    /// Updates an existing tenant in the repository.
    /// </summary>
    /// <param name="tenant">The tenant to update.</param>
    void Update(Tenant tenant);

    /// <summary>
    /// Removes a tenant from the repository.
    /// </summary>
    /// <param name="tenant">The tenant to remove.</param>
    void Remove(Tenant tenant);
}