#nullable enable

namespace DataIngestionService.Application.Models;

/// <summary>
/// Represents an internal data structure carrying a single data point along with
/// essential tenant context for multi-tenancy enforcement.
/// </summary>
/// <remarks>
/// This is intentionally a <c>record struct</c> to optimize for performance in a high-throughput
/// data path by avoiding heap allocations for every data point processed. It is for internal
/// use only and is not part of the public API contract.
/// </remarks>
public readonly record struct EnrichedDataPoint
{
    /// <summary>
    /// The unique identifier of the OPC tag this data point belongs to.
    /// </summary>
    public required Guid OpcTagId { get; init; }

    /// <summary>
    /// The timestamp of the data point, typically from the source OPC server.
    /// This should always be in UTC.
    /// </summary>
    public required DateTime Timestamp { get; init; }

    /// <summary>
    /// The numerical value of the data point.
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// The quality of the data point, conforming to OPC standards.
    /// </summary>
    public required uint Quality { get; init; }

    /// <summary>
    /// The unique identifier of the tenant that owns this data point.
    /// This is crucial for enforcing data isolation in the database.
    /// </summary>
    public required Guid TenantId { get; init; }
}