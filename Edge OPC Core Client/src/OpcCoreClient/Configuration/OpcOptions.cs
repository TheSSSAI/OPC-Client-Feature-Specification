using System.ComponentModel.DataAnnotations;

namespace System.Edge.OpcCoreClient.Configuration;

/// <summary>
/// Provides strongly-typed configuration for connecting to OPC servers.
/// This class is designed to be used with the .NET IOptions pattern.
/// </summary>
public sealed class OpcOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Opc";

    /// <summary>
    /// The endpoint URL for the primary OPC UA server.
    /// Required for operation.
    /// Example: "opc.tcp://localhost:4840"
    /// </summary>
    [Required]
    [Url]
    public string PrimaryServerEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// The endpoint URL for the backup/redundant OPC UA server.
    /// Optional. If provided, enables high-availability failover as per REQ-1-045.
    /// </summary>
    [Url]
    public string? BackupServerEndpoint { get; set; }

    /// <summary>
    /// The desired OPC UA security policy.
    /// Example: "Basic256Sha256"
    /// </summary>
    public string SecurityPolicy { get; set; } = "Basic256Sha256";

    /// <summary>
    /// The authentication type to use for the OPC UA session.
    /// Supported values: "Anonymous", "UsernamePassword"
    /// </summary>
    public string AuthenticationType { get; set; } = "Anonymous";

    /// <summary>
    /// The username for the OPC UA session if AuthenticationType is "UsernamePassword".
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// The password for the OPC UA session if AuthenticationType is "UsernamePassword".
    /// This should be stored securely (e.g., via secrets management).
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// A list of OPC tag identifiers (Node IDs) to subscribe to.
    /// Required for data acquisition.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<string> TagList { get; set; } = new();

    /// <summary>
    /// The interval in milliseconds at which the OPC server should sample data.
    /// </summary>
    [Range(100, 60000)]
    public int SamplingInterval { get; set; } = 1000;

    /// <summary>
    /// The interval in milliseconds at which the client should publish data changes.
    /// </summary>
    [Range(100, 60000)]
    public int PublishingInterval { get; set; } = 1000;
}