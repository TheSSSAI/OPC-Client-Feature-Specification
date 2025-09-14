using MediatR;
using System.Services.IdentityAccess.Application.Shared.Common;
using System.Services.IdentityAccess.Application.Shared.Dtos;

namespace System.Services.IdentityAccess.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Query to retrieve a user's details by their unique identifier.
/// This query is used to fetch application-specific user data.
/// The handler for this query must enforce tenant isolation, ensuring a user from one tenant
/// cannot query details for a user in another tenant.
/// </summary>
/// <param name="UserId">The unique identifier of the user to retrieve.</param>
public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;