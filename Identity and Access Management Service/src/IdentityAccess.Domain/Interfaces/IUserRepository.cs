using System.Services.IdentityAccess.Domain.Entities;

namespace System.Services.IdentityAccess.Domain.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages User entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user with the specified email address exists.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if a user with the email exists; otherwise, false.</returns>
    Task<bool> UserWithEmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user to add.</param>
    void Add(User user);

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    /// <param name="user">The user to update.</param>
    void Update(User user);
    
    /// <summary>
    /// Removes a user from the repository.
    /// </summary>
    /// <param name="user">The user to remove.</param>
    void Remove(User user);
}