using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Services.DeviceManagement.Application.Services;
using System.Services.DeviceManagement.Api.DTOs;
using System.Services.DeviceManagement.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Services.DeviceManagement.Application.Exceptions;

namespace System.Services.DeviceManagement.Api.Endpoints;

/// <summary>
/// Defines the API endpoints for managing OPC Core Client instances.
/// This includes listing clients, retrieving details, and pushing updates.
/// </summary>
public static class ClientEndpoints
{
    private const string AdminPolicy = "AdminPolicy";
    
    public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        var clientsGroup = app.MapGroup("/api/v1/clients")
            .RequireAuthorization(AdminPolicy)
            .WithTags("Clients");

        clientsGroup.MapGet("/", GetAllClientsForTenant)
            .WithName("GetAllClients")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all OPC Core Clients for the tenant.",
                Description = "Retrieves a list of all registered OPC Core Clients for the administrator's tenant, including their status and version."
            });
            
        clientsGroup.MapGet("/{clientId:guid}", GetClientById)
            .WithName("GetClientById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a specific OPC Core Client by ID.",
                Description = "Retrieves detailed information for a single OPC Core Client."
            });

        clientsGroup.MapPost("/{clientId:guid}/config", UpdateClientConfiguration)
            .WithName("UpdateClientConfiguration")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update the configuration for a client.",
                Description = "Pushes a new configuration to a specific OPC Core Client. This is an asynchronous operation."
            })
            .Produces(StatusCodes.Status202Accepted);

        clientsGroup.MapPost("/{clientId:guid}/software-update", UpdateClientSoftware)
            .WithName("UpdateClientSoftware")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Initiate a software update for a client.",
                Description = "Commands a client to download and apply a new software version. This is an asynchronous operation."
            })
            .Produces(StatusCodes.Status202Accepted);

        clientsGroup.MapPost("/{clientId:guid}/rollback", RollbackClientSoftware)
            .WithName("RollbackClientSoftware")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Initiate a software rollback for a client.",
                Description = "Commands a client to roll back to its previously installed software version. This is an asynchronous operation."
            })
            .Produces(StatusCodes.Status202Accepted);
            
        return app;
    }

    private static async Task<Ok<IEnumerable<OpcCoreClient>>> GetAllClientsForTenant(
        [FromServices] IDeviceService deviceService,
        HttpContext httpContext,
        ILogger<OpcCoreClient> logger)
    {
        var tenantId = GetTenantId(httpContext);
        logger.LogInformation("Fetching all clients for Tenant ID: {TenantId}", tenantId);
        var clients = await deviceService.GetClientsByTenantAsync(tenantId);
        return TypedResults.Ok(clients);
    }
    
    private static async Task<Results<Ok<OpcCoreClient>, NotFound, ForbidHttpResult>> GetClientById(
        Guid clientId,
        [FromServices] IDeviceService deviceService,
        HttpContext httpContext,
        ILogger<OpcCoreClient> logger)
    {
        var tenantId = GetTenantId(httpContext);
        logger.LogInformation("Fetching client {ClientId} for Tenant ID: {TenantId}", clientId, tenantId);

        var client = await deviceService.GetClientByIdAsync(clientId);

        if (client == null)
        {
            logger.LogWarning("Client with ID {ClientId} not found.", clientId);
            return TypedResults.NotFound();
        }

        if (client.TenantId != tenantId)
        {
            logger.LogWarning("User from Tenant {TenantId} attempted to access client {ClientId} from Tenant {ClientTenantId}.", tenantId, clientId, client.TenantId);
            return TypedResults.Forbid();
        }

        return TypedResults.Ok(client);
    }

    private static async Task<Results<Accepted, NotFound, BadRequest<string>>> UpdateClientConfiguration(
        Guid clientId,
        [FromBody] UpdateConfigurationRequest request,
        [FromServices] IDeviceService deviceService,
        HttpContext httpContext,
        ILogger<OpcCoreClient> logger)
    {
        var tenantId = GetTenantId(httpContext);
        logger.LogInformation("Attempting to update configuration for Client {ClientId} in Tenant {TenantId}", clientId, tenantId);

        try
        {
            await deviceService.UpdateConfigurationAsync(tenantId, clientId, request.ConfigurationJson);
            return TypedResults.Accepted($"/api/v1/clients/{clientId}");
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Update configuration failed: Client {ClientId} not found for tenant {TenantId}.", clientId, tenantId);
            return TypedResults.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Update configuration failed for Client {ClientId} due to invalid state.", clientId);
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating configuration for Client {ClientId}", clientId);
            return TypedResults.BadRequest("An internal error occurred.");
        }
    }
    
    private static async Task<Results<Accepted, NotFound, BadRequest<string>>> UpdateClientSoftware(
        Guid clientId,
        [FromBody] SoftwareUpdateRequest request,
        [FromServices] IDeviceService deviceService,
        HttpContext httpContext,
        ILogger<OpcCoreClient> logger)
    {
        var tenantId = GetTenantId(httpContext);
        logger.LogInformation("Attempting to update software for Client {ClientId} in Tenant {TenantId} to version {Version}", clientId, tenantId, request.ImageUrl);

        try
        {
            await deviceService.UpdateSoftwareAsync(tenantId, clientId, request.ImageUrl, request.Checksum);
            return TypedResults.Accepted($"/api/v1/clients/{clientId}");
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Software update failed: Client {ClientId} not found for tenant {TenantId}.", clientId, tenantId);
            return TypedResults.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Software update failed for Client {ClientId} due to invalid state.", clientId);
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating software for Client {ClientId}", clientId);
            return TypedResults.BadRequest("An internal error occurred.");
        }
    }
    
    private static async Task<Results<Accepted, NotFound, BadRequest<string>>> RollbackClientSoftware(
        Guid clientId,
        [FromServices] IDeviceService deviceService,
        HttpContext httpContext,
        ILogger<OpcCoreClient> logger)
    {
        var tenantId = GetTenantId(httpContext);
        logger.LogInformation("Attempting to roll back software for Client {ClientId} in Tenant {TenantId}", clientId, tenantId);

        try
        {
            await deviceService.RollbackSoftwareAsync(tenantId, clientId);
            return TypedResults.Accepted($"/api/v1/clients/{clientId}");
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Software rollback failed: Client {ClientId} not found for tenant {TenantId}.", clientId, tenantId);
            return TypedResults.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Software rollback failed for Client {ClientId} due to invalid state.", clientId);
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while rolling back software for Client {ClientId}", clientId);
            return TypedResults.BadRequest("An internal error occurred.");
        }
    }

    private static Guid GetTenantId(HttpContext httpContext)
    {
        var tenantIdClaim = httpContext.User.FindFirstValue("tenant_id");
        if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out var tenantId))
        {
            throw new UnauthorizedAccessException("Tenant ID is missing or invalid in the user's token.");
        }
        return tenantId;
    }
}

// DTO for Software Update, can be moved to DTOs folder if it grows
public record SoftwareUpdateRequest(string ImageUrl, string? Checksum);