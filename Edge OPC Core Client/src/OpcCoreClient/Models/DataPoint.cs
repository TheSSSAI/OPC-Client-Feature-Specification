namespace System.Edge.OpcCoreClient.Models;

/// <summary>
/// Represents a single time-series data point collected from an OPC server.
/// This is a core Data Transfer Object (DTO) used throughout the application,
/// from data acquisition to buffering and cloud transmission.
/// Implemented as a readonly record struct for performance and immutability.
/// </summary>
public readonly record struct DataPoint
{
    /// <summary>
    /// The unique identifier of the OPC tag (e.g., Node ID).
    /// </summary>
    public required string TagId { get; init; }

    /// <summary>
    /// The timestamp of the data point, as reported by the source OPC server.
    /// Using DateTimeOffset to ensure timezone information is preserved.
    /// </summary>
    public required DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// The value of the data point. Stored as 'object' to accommodate various
    /// OPC data types (e.g., int, float, bool, string).
    /// </summary>
    public required object Value { get; init; }

    /// <summary>
    /// The quality status code of the data point, as reported by the OPC server.
    /// Corresponds to OPC UA status codes.
    /// </summary>
    public required ushort Quality { get; init; }
}