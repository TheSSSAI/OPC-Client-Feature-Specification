namespace System.Services.DeviceManagement.Infrastructure.Configuration
{
    /// <summary>
    /// Provides strongly-typed configuration for connecting to the MQTT broker,
    /// populated from appsettings.json using the ASP.NET Core Options Pattern.
    /// This configuration is essential for the service to establish its command and control channel.
    /// </summary>
    public class MqttSettings
    {
        /// <summary>
        /// The configuration section name in appsettings.json.
        /// </summary>
        public const string SectionName = "Mqtt";

        /// <summary>
        /// The hostname or IP address of the MQTT broker.
        /// </summary>
        public string BrokerAddress { get; init; } = string.Empty;

        /// <summary>
        /// The port for the MQTT broker. Typically 1883 for non-secure and 8883 for secure (TLS) connections.
        /// </summary>
        public int Port { get; init; }

        /// <summary>
        /// The username for the Device Management Service to authenticate with the MQTT broker.
        /// This is for the service's own connection, not for individual devices.
        /// Can be left empty for anonymous access if the broker allows it.
        /// </summary>
        public string? Username { get; init; }

        /// <summary>
        /// The password for the Device Management Service to authenticate with the MQTT broker.
        /// It is strongly recommended to load this value from a secure secret store (e.g., AWS Secrets Manager, Azure Key Vault).
        /// </summary>
        public string? Password { get; init; }

        /// <summary>
        /// The unique Client ID for the Device Management Service instance.
        /// If running multiple instances, each should have a unique ID, often with a random suffix.
        /// </summary>
        public string ClientId { get; init; } = "DeviceManagementService-";
    }
}