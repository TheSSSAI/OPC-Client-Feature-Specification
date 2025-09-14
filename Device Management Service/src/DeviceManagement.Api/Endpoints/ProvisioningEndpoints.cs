using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Services.DeviceManagement.Application.Services;
using System.Services.DeviceManagement.Api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Services.DeviceManagement.Application.Exceptions;

namespace System.Services.DeviceManagement.Api.Endpoints;

/// <summary>
/// Defines the API endpoints for provisioning new OPC Core Client instances.
/// This includes a secure endpoint for administrators to generate tokens
/// and a public endpoint for clients to register themselves.
/// </summary>
public static class ProvisioningEndpoints
{
    private const string AdminPolicy = "AdminPolicy";
    private const string ProvisioningRateLimiter = "provisioning";

    public static IEndpointRouteBuilder MapProvisioningEndpoints(this IEndpointRouteBuilder app)
    {
        var adminGroup = app.MapGroup("/api/v1/clients/provision-token")
            .RequireAuthorization(AdminPolicy)
            .WithTags("Provisioning");

        adminGroup.MapPost("/", GenerateProvisioningToken)
            .WithName("GenerateProvisioningToken")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Generate a single-use provisioning token.",
                Description = "Creates a new client record in a 'Pending' state and generates a secure, time-limited, single-use token for it."
            });

        var publicGroup = app.MapGroup("/provision")
            .WithTags("Provisioning");

        publicGroup.MapPost("/register", RegisterClient)
            .WithName("RegisterClient")
            .RequireRateLimiting(ProvisioningRateLimiter)
            .AllowAnonymous()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Register a new client using a provisioning token.",
                Description = "Public endpoint for a new OPC Core Client to exchange a valid one-time token and a Certificate Signing Request (CSR) for a long-term client certificate."
            });
            
        return app;
    }

    private static async Task<Results<Created<ProvisioningTokenResponse>, BadRequest<string>>> GenerateProvisioningToken(
        [FromBody] ProvisionTokenRequest request,
        [FromServices] IProvisioningService provisioningService,
        HttpContext httpContext,
        ILogger<IProvisioningService> logger)
    {
        var tenantId = GetTenantId(httpContext);
        logger.LogInformation("Administrator from Tenant {TenantId} is provisioning a new client named '{ClientName}'.", tenantId, request.ClientName);
        
        try
        {
            var (token, expiry) = await provisioningService.GenerateProvisioningTokenAsync(tenantId, request.ClientName);
            var response = new ProvisioningTokenResponse(token, expiry);
            return TypedResults.Created($"/api/v1/clients/provision-token", response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating provisioning token for tenant {TenantId}", tenantId);
            return TypedResults.BadRequest("An internal error occurred while generating the token.");
        }
    }

    private static async Task<Results<ContentHttpResult, UnauthorizedHttpResult, BadRequest<string>, Conflict<string>>> RegisterClient(
        [FromBody] ClientRegistrationRequest request,
        [FromServices] IProvisioningService provisioningService,
        ILogger<IProvisioningService> logger)
    {
        logger.LogInformation("Received a client registration request.");

        if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.Csr))
        {
            return TypedResults.BadRequest("Token and CSR are required.");
        }

        try
        {
            var signedCertificate = await provisioningService.RegisterClientAsync(request.Token, request.Csr);
            logger.LogInformation("Successfully registered a new client.");
            return TypedResults.Content(signedCertificate, "application/x-pem-file");
        }
        catch (InvalidProvisioningTokenException ex)
        {
            logger.LogWarning(ex, "Client registration failed due to invalid token.");
            return TypedResults.Unauthorized();
        }
        catch (ProvisioningTokenConflictException ex)
        {
            logger.LogWarning(ex, "Client registration failed due to token conflict (already used).");
            return TypedResults.Conflict(ex.Message);
        }
        catch (InvalidCsrException ex)
        {
            logger.LogWarning(ex, "Client registration failed due to invalid CSR.");
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred during client registration.");
            return TypedResults.BadRequest("An internal error occurred during registration.");
        }
    }

    private static Guid GetTenantId(HttpContext httpContext)
    {
        var tenantIdClaim = httpContext.User.FindFirstValue("tenant_id");
        if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out var tenantId))
        {
            // This should not happen if RequireAuthorization is working, but it's a good safeguard.
            throw new UnauthorizedAccessException("Tenant ID is missing or invalid in the user's token.");
        }
        return tenantId;
    }
}

// DTOs for Provisioning
public record ProvisioningTokenResponse(string Token, DateTime ExpiryUtc);
public record ClientRegistrationRequest(string Token, string Csr);