namespace System.Services.IdentityAccess.Domain.Interfaces;

/// <summary>
/// Defines the contract for the Unit of Work pattern, which coordinates the work of multiple repositories.
/// It ensures that all changes within a single business transaction are committed together.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for managing User entities.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Gets the repository for managing Tenant entities.
    /// </summary>
    ITenantRepository Tenants { get; }

    /// <summary>
    /// Gets the repository for managing Role entities.
    /// </summary>
    IRoleRepository Roles { get; }

    /// <summary>
    /// Gets the repository for managing License entities.
    /// </summary>
    ILicenseRepository Licenses { get; }
    
    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the underlying database.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. 
    /// The task result contains the number of state entries written to the database.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}