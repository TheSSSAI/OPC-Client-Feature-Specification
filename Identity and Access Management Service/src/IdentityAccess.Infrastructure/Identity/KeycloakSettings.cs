namespace System.Services.IdentityAccess.Infrastructure.Identity;

/// <summary>
/// Represents the configuration settings required to interact with the Keycloak Admin API.
/// This class is designed to be populated from the application's configuration (e.g., appsettings.json).
/// </summary>
public class KeycloakSettings
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Keycloak";

    /// <summary>
    /// Gets or sets the base URL for the Keycloak Admin API.
    /// e.g., "http://localhost:8081/admin/realms/"
    /// </summary>
    public string AdminApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the realm to administer.
    /// e.g., "master" or a custom realm name.
    /// </summary>
    public string Realm { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client ID of the service account used for authenticating with the Admin API.
    /// This client must have the necessary permissions (e.g., manage-users) in Keycloak.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client secret for the service account client.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}