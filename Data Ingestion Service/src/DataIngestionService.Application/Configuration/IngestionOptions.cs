using System.ComponentModel.DataAnnotations;

#nullable enable

namespace DataIngestionService.Application.Configuration;

/// <summary>
/// Provides strongly-typed access to application settings related to the data ingestion pipeline,
/// specifically for batching and performance tuning.
/// </summary>
public sealed class IngestionOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Ingestion";

    /// <summary>
    /// The maximum number of data points to include in a single database write batch.
    /// A larger batch size increases throughput but also increases latency and memory usage.
    /// </summary>
    [Range(100, 100_000, ErrorMessage = "BatchSize must be between 100 and 100,000.")]
    public int BatchSize { get; set; } = 5000;

    /// <summary>
    /// The time interval in seconds at which the background service will flush the
    /// in-memory buffer to the database, even if the batch size is not yet reached.
    /// This controls the maximum latency for data to become persistent.
    /// </summary>
    [Range(1, 60, ErrorMessage = "FlushIntervalSeconds must be between 1 and 60.")]
    public int FlushIntervalSeconds { get; set; } = 1;
}