using System.Edge.OpcCoreClient.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Edge.OpcCoreClient.Persistence;

/// <summary>
/// Defines the contract for the persistent store-and-forward buffer.
/// This component is critical for ensuring zero data loss during network outages, as per REQ-1-079.
/// Implementations are expected to be high-performance, durable, and thread-safe.
/// </summary>
public interface IDataBuffer : IAsyncDisposable
{
    /// <summary>
    /// Initializes the data buffer, preparing it for read/write operations.
    /// This may include opening files, checking for existing data, and setting up internal structures.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the initialization process.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    Task InitializeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously adds a single data point to the end of the buffer.
    /// </summary>
    /// <param name="dataPoint">The data point to be buffered.</param>
    /// <returns>A task that represents the asynchronous enqueue operation.</returns>
    Task EnqueueAsync(DataPoint dataPoint);
    
    /// <summary>
    /// Asynchronously adds a batch of data points to the end of the buffer.
    /// This is often more performant than adding items one by one.
    /// </summary>
    /// <param name="dataPoints">The collection of data points to be buffered.</param>
    /// <returns>A task that represents the asynchronous enqueue operation.</returns>
    Task EnqueueBatchAsync(IEnumerable<DataPoint> dataPoints);

    /// <summary>
    /// Asynchronously retrieves and removes a batch of data points from the start of the buffer.
    /// </summary>
    /// <param name="batchSize">The maximum number of data points to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous dequeue operation. The task result contains a read-only list
    /// of the dequeued data points. If the buffer is empty, an empty list is returned.
    /// </returns>
    Task<IReadOnlyList<DataPoint>> DequeueBatchAsync(int batchSize);
    
    /// <summary>
    /// Gets the approximate number of data points currently stored in the buffer.
    /// </summary>
    /// <returns>The number of items in the buffer.</returns>
    long GetCount();

    /// <summary>
    /// Gets a value indicating whether the buffer is empty.
    /// </summary>
    /// <returns>True if the buffer is empty; otherwise, false.</returns>
    bool IsEmpty();
}