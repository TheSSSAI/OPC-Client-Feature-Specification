using System.ComponentModel.DataAnnotations;

namespace System.Edge.OpcCoreClient.Configuration;

/// <summary>
/// Provides strongly-typed configuration for connecting to the MQTT broker for command, control, and status messaging.
/// This class is designed to be used with the .NET IOptions pattern.
/// This configuration is driven by REQ-1-010.
/// </summary>
public sealed class MqttOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Mqtt";

    /// <summary>
    /// The hostname or IP address of the MQTT broker.
    /// </summary>
    [Required]
    public string BrokerAddress { get; set; } = string.Empty;

    /// <summary>
    /// The port number for the MQTT broker.
    /// </summary>
    [Range(1, 65535)]
    public int Port { get; set; } = 8883;

    /// <summary>
    /// The unique client ID for this OPC Core Client instance.
    /// If not provided, a unique ID will be generated.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Specifies whether to use a secure TLS connection to the broker.
    /// Defaults to true for production security.
    /// </summary>
    public bool UseTls { get; set; } = true;

    /// <summary>
    /// The base topic structure for this client.
    /// Placeholders {tenantId} and {clientId} will be replaced at runtime.
    /// </summary>
    [Required]
    public string BaseTopic { get; set; } = "tenants/{tenantId}/clients/{clientId}";
    
    /// <summary>
    /// The sub-topic for receiving commands.
    /// </summary>
    [Required]
    public string CommandTopicSuffix { get; set; } = "commands";
    
    /// <summary>
    /// The sub-topic for publishing status updates.
    /// </summary>
    [Required]
    public string StatusTopicSuffix { get; set; } = "status";
    
    /// <summary>
    /// The sub-topic for publishing events like anomalies.
    /// </summary>
    [Required]
    public string EventTopicSuffix { get; set; } = "events/anomaly";
}