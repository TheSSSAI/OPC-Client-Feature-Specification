namespace System.Services.DeviceManagement.Application.Interfaces;

/// <summary>
/// Defines a contract for publishing command messages to the MQTT broker for consumption by edge clients.
/// This interface abstracts the underlying MQTT client implementation from the application logic,
/// adhering to the Dependency Inversion Principle.
/// </summary>
public interface IMqttCommandPublisher
{
    /// <summary>
    /// Publishes a command to a specific client to update its configuration.
    /// This operation is asynchronous and fire-and-forget from the perspective of the application service.
    /// The client is expected to report its status back on a separate status topic.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant the client belongs to. Used to construct the MQTT topic.</param>
    /// <param name="clientId">The unique identifier of the target client.</param>
    /// <param name="newConfigurationJson">The new configuration payload as a JSON string.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous publish operation.</returns>
    Task PublishConfigurationUpdateAsync(Guid tenantId, Guid clientId, string newConfigurationJson, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a command to a specific client to trigger a remote software update.
    /// The client is expected to download the specified image, apply the update, and report its status.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant the client belongs to. Used to construct the MQTT topic.</param>
    /// <param name="clientId">The unique identifier of the target client.</param>
    /// <param name="imageUrl">The URL or tag of the new software image (e.g., a Docker image URI).</param>
    /// <param name="checksum">A checksum (e.g., SHA256) of the software package for integrity verification by the client.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous publish operation.</returns>
    Task PublishSoftwareUpdateAsync(Guid tenantId, Guid clientId, string imageUrl, string checksum, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a command to a specific client to initiate a rollback to its previous software version.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant the client belongs to.</param>
    /// <param name="clientId">The unique identifier of the target client.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous publish operation.</returns>
    Task PublishSoftwareRollbackAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Publishes a command to a specific client to request its log files.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant the client belongs to.</param>
    /// <param name="clientId">The unique identifier of the target client.</param>
    /// <param name="uploadUrl">A pre-signed URL where the client should upload its logs.</param>
    /// <param name="correlationId">A correlation ID to track the request and response.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous publish operation.</returns>
    Task PublishLogRetrievalRequestAsync(Guid tenantId, Guid clientId, string uploadUrl, string correlationId, CancellationToken cancellationToken = default);
}