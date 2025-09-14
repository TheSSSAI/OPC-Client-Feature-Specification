using System.Collections.Concurrent;
using System.Collections.Generic;
using DataIngestionService.Application.Models;
using Microsoft.Extensions.Options;
using DataIngestionService.Application.Configuration;

namespace DataIngestionService.Application.Services
{
    /// <summary>
    /// A thread-safe, in-memory buffer for incoming enriched data points.
    /// This service is registered as a singleton to be shared between the gRPC service (producer)
    /// and the background flushing service (consumer).
    /// </summary>
    public class DataPointBatchingService
    {
        private readonly ConcurrentQueue<EnrichedDataPoint> _buffer = new();
        private readonly int _batchSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointBatchingService"/> class.
        /// </summary>
        /// <param name="ingestionOptions">Configuration options for ingestion, including batch size.</param>
        public DataPointBatchingService(IOptions<IngestionOptions> ingestionOptions)
        {
            _batchSize = ingestionOptions.Value.BatchSize;
        }
        
        /// <summary>
        /// Gets the current number of data points in the buffer.
        /// </summary>
        public int CurrentBufferSize => _buffer.Count;

        /// <summary>
        /// Adds a single enriched data point to the buffer. This method is designed to be
        /// called frequently by multiple producer threads and is optimized for high-throughput.
        /// </summary>
        /// <param name="dataPoint">The enriched data point to add to the queue.</param>
        public void AddDataPoint(EnrichedDataPoint dataPoint)
        {
            _buffer.Enqueue(dataPoint);
        }

        /// <summary>
        /// Drains a batch of enriched data points from the buffer.
        /// This method attempts to dequeue up to the configured batch size.
        /// It is designed to be called by a single consumer thread.
        /// </summary>
        /// <returns>A read-only collection of enriched data points, which may be empty if the buffer is empty.</returns>
        public IReadOnlyCollection<EnrichedDataPoint> DrainBatch()
        {
            if (_buffer.IsEmpty)
            {
                return System.Array.Empty<EnrichedDataPoint>();
            }

            var batch = new List<EnrichedDataPoint>(_batchSize);
            
            for (int i = 0; i < _batchSize; i++)
            {
                if (_buffer.TryDequeue(out var dataPoint))
                {
                    batch.Add(dataPoint);
                }
                else
                {
                    // Buffer is empty, stop draining.
                    break;
                }
            }

            return batch;
        }
    }
}