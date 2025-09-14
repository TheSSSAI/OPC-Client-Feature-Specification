using MediatR;
using System.Services.IdentityAccess.Application.Shared.Common;
using System.Services.IdentityAccess.Application.Shared.Dtos;

namespace System.Services.IdentityAccess.Application.Features.Tenants.Commands.CreateTenant;

/// <summary>
/// Command to onboard a new tenant into the system.
/// This command includes not only the tenant's name but also the details for creating
/// the tenant's initial administrator user. The handler for this command will
/// create the Tenant, the User, assign the 'Administrator' role, and set up a default license.
/// </summary>
/// <param name="TenantName">The desired name for the new tenant. Must be unique.</param>
/// <param name="AdminFirstName">The first name of the initial administrator for the tenant.</param>
/// <param name="AdminLastName">The last name of the initial administrator for the tenant.</param>
/// <param name="AdminEmail">The email address of the initial administrator. This will be their username.</param>
/// <param name="AdminPassword">The initial password for the administrator account.</param>
public record CreateTenantCommand(
    string TenantName,
    string AdminFirstName,
    string AdminLastName,
    string AdminEmail,
    string AdminPassword) : IRequest<Result<TenantDto>>;