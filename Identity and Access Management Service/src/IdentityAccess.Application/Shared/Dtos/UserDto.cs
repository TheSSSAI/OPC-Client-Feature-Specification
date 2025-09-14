namespace System.Services.IdentityAccess.Application.Shared.Dtos;

/// <summary>
/// Data Transfer Object for a User.
/// </summary>
/// <param name="Id">The unique identifier of the user, corresponding to the Keycloak subject ID.</param>
/// <param name="TenantId">The unique identifier of the tenant the user belongs to.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="FirstName">The user's first name.</param>
/// <param name="LastName">The user's last name.</param>
/// <param name="Roles">The list of roles assigned to the user.</param>
public record UserDto(
    Guid Id,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    List<string> Roles);