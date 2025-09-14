using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace System.Services.DeviceManagement.Api.DTOs;

/// <summary>
/// Represents the request body for an administrator to generate a new provisioning token
/// for an OPC Core Client.
/// </summary>
/// <param name="ClientName">A user-friendly name for the new client instance. This name will be associated with the client in the management dashboard.</param>
public record ProvisionTokenRequest(
    [property: Required(ErrorMessage = "Client name is required.")]
    [property: StringLength(100, MinimumLength = 3, ErrorMessage = "Client name must be between 3 and 100 characters.")]
    [property: JsonPropertyName("clientName")]
    string ClientName
);