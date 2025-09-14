using System.ComponentModel.DataAnnotations;

namespace System.Edge.OpcCoreClient.Configuration;

/// <summary>
/// Provides strongly-typed configuration for the on-disk data buffer.
/// This class is designed to be used with the .NET IOptions pattern.
/// This configuration fulfills REQ-1-079.
/// </summary>
public sealed class BufferOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Buffer";

    /// <summary>
    /// The maximum size of the on-disk buffer in bytes.
    /// Defaults to 1 GB (1 * 1024 * 1024 * 1024).
    /// </summary>
    [Range(10 * 1024 * 1024, 2L * 1024 * 1024 * 1024)] // 10MB to 2GB
    public long MaxSizeBytes { get; set; } = 1 * 1024 * 1024 * 1024;

    /// <summary>
    /// The path to the directory where the buffer file(s) will be stored.
    /// If not specified, a default path within the application's data directory will be used.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// The name of the buffer file.
    /// </summary>
    [Required]
    public string FileName { get; set; } = "datapoints.buffer";
}