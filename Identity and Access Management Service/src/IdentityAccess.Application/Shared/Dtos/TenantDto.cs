namespace System.Services.IdentityAccess.Application.Shared.Dtos;

/// <summary>
/// Data Transfer Object for a Tenant.
/// </summary>
/// <param name="Id">The unique identifier of the tenant.</param>
/// <param name="Name">The name of the tenant.</param>
public record TenantDto(
    Guid Id,
    string Name);