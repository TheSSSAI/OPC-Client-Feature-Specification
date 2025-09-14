using System.Services.IdentityAccess.Domain.Entities;

namespace System.Services.IdentityAccess.Domain.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages Role entities.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Gets a role by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the role.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The role if found; otherwise, null.</returns>
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role by its name.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The role if found; otherwise, null.</returns>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all roles for a given tenant.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of all roles for the current tenant context.</returns>
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a new role to the repository.
    /// </summary>
    /// <param name="role">The role to add.</param>
    void Add(Role role);
}