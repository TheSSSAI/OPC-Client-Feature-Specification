using MediatR;
using System.Services.IdentityAccess.Application.Shared.Common;
using System.Services.IdentityAccess.Application.Shared.Dtos;

namespace System.Services.IdentityAccess.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Command to create a new user within a tenant.
/// This command encapsulates all the necessary information to provision a user
/// both in the local application database and in the external Identity Provider (Keycloak).
/// The handler for this command will also check license limits before proceeding.
/// </summary>
/// <param name="TenantId">The ID of the tenant where the user will be created. This is typically derived from the administrator's session.</param>
/// <param name="FirstName">The user's first name.</param>
/// <param name="LastName">The user's last name.</param>
/// <param name="Email">The user's email address. Must be unique within the tenant.</param>
/// <param name="Password">The user's initial password. This will be sent to Keycloak and not stored locally.</param>
/// <param name="Roles">A list of role names to assign to the user upon creation.</param>
public record CreateUserCommand(
    Guid TenantId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    List<string> Roles) : IRequest<Result<UserDto>>;