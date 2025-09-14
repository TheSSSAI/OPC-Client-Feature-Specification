using DeviceManagement.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceManagement.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that handles data access for ProvisioningToken entities.
    /// These tokens are used for the secure, one-time bootstrapping of new OPC Core Clients.
    /// </summary>
    public interface IProvisioningTokenRepository
    {
        /// <summary>
        /// Adds a new provisioning token to the repository.
        /// </summary>
        /// <param name="token">The provisioning token entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(ProvisioningToken token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a provisioning token by its hash. This is the primary method for validating a token
        /// during the client registration process.
        /// </summary>
        /// <param name="tokenHash">The SHA256 hash of the plaintext token.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token if found; otherwise, null.</returns>
        Task<ProvisioningToken?> FindByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates an existing provisioning token in the repository.
        /// This is typically used to mark a token as used.
        /// </summary>
        /// <param name="token">The token entity with updated values.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(ProvisioningToken token, CancellationToken cancellationToken = default);
    }
}