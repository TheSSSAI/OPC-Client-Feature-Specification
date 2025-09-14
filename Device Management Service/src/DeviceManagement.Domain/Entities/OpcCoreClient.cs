using System;
using System.Services.DeviceManagement.Domain.Enums;

namespace System.Services.DeviceManagement.Domain.Entities
{
    /// <summary>
    /// Represents a registered OPC Core Client instance. This is the central aggregate root for the
    /// device management domain, tracking the identity, status, configuration, and version of each edge device.
    /// </summary>
    public class OpcCoreClient
    {
        /// <summary>
        /// The unique identifier for the client instance, acting as the primary key.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The identifier of the tenant this client belongs to, ensuring strict data isolation.
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// A user-friendly, mutable name for the client instance (e.g., "Site A - Packaging Line 1").
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The current operational status of the client, updated via MQTT status messages.
        /// </summary>
        public ClientStatus Status { get; private set; }

        /// <summary>
        /// The UTC timestamp of the last message (e.g., heartbeat) received from the client.
        /// This is used to determine if a client has gone offline.
        /// </summary>
        public DateTime? LastSeenUtc { get; private set; }

        /// <summary>
        /// The current software version (e.g., Docker image tag) running on the client.
        /// </summary>
        public string? SoftwareVersion { get; private set; }

        /// <summary>
        /// The client's desired state configuration, stored as a JSON string. This configuration
        /// is pushed from the Central Management Plane.
        /// </summary>
        public string? ConfigurationJson { get; private set; }
        
        /// <summary>
        /// The Common Name (CN) from the client's X.509 certificate. This must be unique across all clients
        /// and is used as the Client ID for MQTT authentication.
        /// </summary>
        public string? CertificateCommonName { get; private set; }
        
        /// <summary>
        /// EF Core constructor.
        /// </summary>
        private OpcCoreClient() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OpcCoreClient"/> class for a new provisioning request.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant owning this client.</param>
        /// <param name="name">The initial user-friendly name for the client.</param>
        public OpcCoreClient(Guid tenantId, string name)
        {
            if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Client name cannot be null or whitespace.", nameof(name));
            }

            Id = Guid.NewGuid();
            TenantId = tenantId;
            Name = name;
            Status = ClientStatus.Pending;
            LastSeenUtc = null;
            SoftwareVersion = null;
            ConfigurationJson = "{}"; // Default to empty JSON object
            CertificateCommonName = null;
        }

        /// <summary>
        /// Completes the provisioning process by assigning the certificate common name and moving the status to Offline.
        /// </summary>
        /// <param name="commonName">The unique common name from the client's new certificate.</param>
        public void CompleteProvisioning(string commonName)
        {
            if (Status != ClientStatus.Pending)
            {
                throw new InvalidOperationException("Cannot complete provisioning for a client that is not in a 'Pending' state.");
            }
            if (string.IsNullOrWhiteSpace(commonName))
            {
                throw new ArgumentException("Certificate common name cannot be null or whitespace.", nameof(commonName));
            }
            
            CertificateCommonName = commonName;
            Status = ClientStatus.Offline;
            LastSeenUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the client's status to Online and records the current timestamp.
        /// </summary>
        /// <param name="reportedVersion">The software version reported by the client.</param>
        public void MarkAsOnline(string reportedVersion)
        {
            if (string.IsNullOrWhiteSpace(reportedVersion))
            {
                throw new ArgumentException("Reported software version cannot be null or whitespace.", nameof(reportedVersion));
            }

            Status = ClientStatus.Online;
            SoftwareVersion = reportedVersion;
            LastSeenUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the client's status to Offline.
        /// </summary>
        public void MarkAsOffline()
        {
            if (Status != ClientStatus.Error)
            {
                Status = ClientStatus.Offline;
            }
            // We don't update LastSeenUtc here, as it should reflect the last time it was actually seen.
        }

        /// <summary>
        /// Applies a new configuration to the client. This represents the desired state.
        /// </summary>
        /// <param name="newConfigurationJson">The new configuration as a JSON string.</param>
        public void UpdateConfiguration(string newConfigurationJson)
        {
            if (string.IsNullOrWhiteSpace(newConfigurationJson))
            {
                throw new ArgumentException("Configuration JSON cannot be null or whitespace.", nameof(newConfigurationJson));
            }
            ConfigurationJson = newConfigurationJson;
        }
        
        /// <summary>
        /// Renames the client.
        /// </summary>
        /// <param name="newName">The new user-friendly name for the client.</param>
        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("New name cannot be null or whitespace.", nameof(newName));
            }
            Name = newName;
        }
    }
}