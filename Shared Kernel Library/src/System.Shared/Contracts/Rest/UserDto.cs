using System.Text.Json.Serialization;

namespace System.Shared.Contracts.Rest;

/// <summary>
/// Represents a user for data transfer purposes in REST APIs.
/// This DTO provides a stable, public contract for user information.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="FirstName">The user's first name.</param>
/// <param name="LastName">The user's last name.</param>
/// <param name="Roles">The roles assigned to the user.</param>
public record UserDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName,
    [property: JsonPropertyName("roles")] List<string> Roles
);