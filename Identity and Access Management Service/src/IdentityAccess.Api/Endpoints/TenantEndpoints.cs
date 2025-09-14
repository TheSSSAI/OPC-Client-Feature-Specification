using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Services.IdentityAccess.Api.Shared;
using System.Services.IdentityAccess.Application.Features.Tenants.Commands.CreateTenant;
using System.Services.IdentityAccess.Application.Features.Tenants.Queries.GetTenantById;
using System.Services.IdentityAccess.Application.Features.Tenants.Queries.GetTenantLicense;

namespace System.Services.IdentityAccess.Api.Endpoints;

public static class TenantEndpoints
{
    public static IEndpointRouteBuilder MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var tenants = app.MapGroup("/api/v1/tenants")
            .RequireAuthorization("SuperAdminPolicy") // Tenant management is a high-level administrative task
            .WithTags("Tenants");

        tenants.MapPost("", async (
            [FromBody] CreateTenantCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("CreateTenant")
        .WithSummary("Creates a new tenant in the system.")
        .WithDescription("Onboards a new tenant, creating their data isolation context and initial license as per REQ-1-024.")
        .Produces(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        tenants.MapGet("{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTenantByIdQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("GetTenantById")
        .WithSummary("Gets a tenant by its unique identifier.")
        .Produces<GetTenantByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        var tenantAdmin = app.MapGroup("/api/v1/tenant")
            .RequireAuthorization("AdminPolicy")
            .WithTags("Tenant Management");
        
        tenantAdmin.MapGet("license", async(
            ISender sender,
            // ICurrentUserProvider currentUserProvider, // Assume this service exists to get tenantId
            CancellationToken cancellationToken) =>
        {
            // In a real implementation, the tenant ID would be sourced from the user's claims
            // via a dedicated service, not passed in the route.
            // For now, let's assume we have a way to get it.
            var tenantId = Guid.NewGuid(); // Placeholder for current user's tenant ID.
            
            var query = new GetTenantLicenseQuery(tenantId);
            var result = await sender.Send(query, cancellationToken);
            return result.ToApiResult();
        })
        .WithName("GetTenantLicense")
        .WithSummary("Gets the license details for the current tenant.")
        .WithDescription("Provides details on the current subscription plan and usage as per REQ-1-063 and US-054.")
        .Produces<GetTenantLicenseResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}