using System;
using System.Threading;
using System.Threading.Tasks;

namespace AssetTopology.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for a Unit of Work, which manages transactions for the application's data context.
    /// This pattern ensures that all repository operations within a single business transaction are committed or rolled back together.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for Asset entities.
        /// </summary>
        IAssetRepository Assets { get; }

        /// <summary>
        /// Gets the repository for AssetTemplate entities.
        /// </summary>
        IAssetTemplateRepository AssetTemplates { get; }

        /// <summary>
        /// Gets the repository for OpcTag entities.
        /// </summary>
        IOpcTagRepository OpcTags { get; }

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}