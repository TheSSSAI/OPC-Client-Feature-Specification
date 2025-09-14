using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Services.IdentityAccess.Api.Shared;
using System.Services.IdentityAccess.Application.Features.Roles.Commands.AssignRoleToUser;
using System.Services.IdentityAccess.Application.Features.Roles.Queries.GetAllRoles;

namespace System.Services.IdentityAccess.Api.Endpoints;

public static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var roles = app.MapGroup("/api/v1/roles")
            .RequireAuthorization()
            .WithTags("Roles");

        roles.MapGet("", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAllRolesQuery();
            var result = await sender.Send(query, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("GetAllRoles")
        .WithSummary("Gets all available roles in the system.")
        .WithDescription("Retrieves the list of predefined and custom roles as per REQ-1-011.")
        .Produces<GetAllRolesResponse>(StatusCodes.Status200OK);
        
        // This endpoint might be better placed under /users/{userId}/roles
        roles.MapPost("assign", async (
            [FromBody] AssignRoleToUserCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("AssignRoleToUser")
        .WithSummary("Assigns a role to a specific user.")
        .WithDescription("Allows an administrator to assign a role, potentially with an asset scope, to a user, fulfilling US-048.")
        .Produces(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("AdminPolicy");

        // Future endpoints for custom role management (US-049) would go here
        // roles.MapPost("/custom", ...)
        // roles.MapPut("/custom/{id}", ...)
        // roles.MapDelete("/custom/{id}", ...)

        return app;
    }
}