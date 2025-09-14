using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Services.IdentityAccess.Api.Shared;
using System.Services.IdentityAccess.Application.Features.Users.Commands.CreateUser;
using System.Services.IdentityAccess.Application.Features.Users.Commands.UpdateNotificationPreferences;
using System.Services.IdentityAccess.Application.Features.Users.Queries.GetNotificationPreferences;
using System.Services.IdentityAccess.Application.Features.Users.Queries.GetUserById;
using System.Security.Claims;

namespace System.Services.IdentityAccess.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("/api/v1/users")
            .RequireAuthorization()
            .WithTags("Users");

        users.MapPost("", async (
            [FromBody] CreateUserCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("CreateUser")
        .WithSummary("Creates a new user account.")
        .WithDescription("Creates a new user, checks license limits, and syncs with the identity provider.")
        .Produces(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .RequireAuthorization("AdminPolicy"); // REQ-USR-001 - Only Admins can create users

        users.MapGet("{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("GetUserById")
        .WithSummary("Gets a user by their unique identifier.")
        .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("AdminPolicy");

        // Placeholder for listing users - would require a GetUsersQuery
        // users.MapGet("", ...);

        users.MapGet("/me/preferences", async (
            ClaimsPrincipal principal,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetNotificationPreferencesQuery(userId);
            var result = await sender.Send(query, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("GetNotificationPreferences")
        .WithSummary("Gets the current user's notification preferences.")
        .WithDescription("Allows a user to retrieve their own notification settings as per REQ-FR-022.")
        .Produces<GetNotificationPreferencesResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        users.MapPut("/me/preferences", async (
            [FromBody] UpdateNotificationPreferencesRequest request,
            ClaimsPrincipal principal,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new UpdateNotificationPreferencesCommand(userId, request.Preferences);
            var result = await sender.Send(command, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("UpdateNotificationPreferences")
        .WithSummary("Updates the current user's notification preferences.")
        .WithDescription("Allows a user to save their notification settings as per REQ-FR-022.")
        .Produces(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}