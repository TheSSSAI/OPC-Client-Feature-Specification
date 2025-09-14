using System.Services.IdentityAccess.Domain.Entities;

namespace System.Services.IdentityAccess.Domain.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages License entities.
/// </summary>
public interface ILicenseRepository
{
    /// <summary>
    /// Gets the license for a specific tenant.
    /// </summary>
    /// <param name="tenantId">The unique identifier of the tenant.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The license associated with the tenant if found; otherwise, null.</returns>
    Task<License?> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new license to the repository.
    /// </summary>
    /// <param name="license">The license to add.</param>
    void Add(License license);

    /// <summary>
    /// Updates an existing license.
    /// </summary>
    /// <param name="license">The license to update.</param>
    void Update(License license);
}