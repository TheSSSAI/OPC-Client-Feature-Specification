using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace System.Services.DeviceManagement.Api.DTOs;

/// <summary>
/// Represents the request body for the API endpoint that triggers a client configuration update.
/// </summary>
/// <param name="ConfigurationJson">
/// The complete configuration for the client, provided as a serialized JSON string.
/// The client is expected to validate this JSON against its known schema upon receipt.
/// </param>
public record UpdateConfigurationRequest(
    [property: Required(ErrorMessage = "Configuration JSON payload is required.")]
    [property: JsonPropertyName("configurationJson")]
    string ConfigurationJson
);