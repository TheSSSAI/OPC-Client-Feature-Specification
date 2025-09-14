using DataIngestionService.Application.Models;

#nullable enable

namespace DataIngestionService.Application.Interfaces;

/// <summary>
/// Defines a contract for persisting batches of enriched time-series data.
/// This interface abstracts the specific database technology and implementation details
/// from the application layer, adhering to Clean Architecture principles.
/// </summary>
public interface IDataPointWriter
{
    /// <summary>
    /// Asynchronously writes a collection of enriched data points to the persistent data store.
    /// The implementation must ensure this operation is atomic (transactional) for the entire batch.
    /// </summary>
    /// <param name="dataPoints">The collection of enriched data points to be persisted.</param>
    /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
    /// <exception cref="System.Exception">
    /// Throws exceptions related to database connectivity or transaction failures after
    /// exhausting any internal resilience policies.
    /// </exception>
    Task WriteBatchAsync(IReadOnlyCollection<EnrichedDataPoint> dataPoints, CancellationToken cancellationToken);
}