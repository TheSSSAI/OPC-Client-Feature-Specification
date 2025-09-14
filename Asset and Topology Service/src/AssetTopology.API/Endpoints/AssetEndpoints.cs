using AssetTopology.Application.Interfaces;
using AssetTopology.Contracts.Assets;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AssetTopology.API.Extensions;

namespace AssetTopology.API.Endpoints;

public static class AssetEndpoints
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var assetGroup = app.MapGroup("/api/v1/assets").WithTags("Assets");

        assetGroup.MapGet("/", GetAssetHierarchy)
            .WithName("GetAssetHierarchy")
            .Produces<AssetNodeDto>()
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");

        assetGroup.MapGet("/{id:guid}", GetAssetById)
            .WithName("GetAssetById")
            .Produces<AssetDto>()
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");

        assetGroup.MapPost("/", CreateAsset)
            .WithName("CreateAsset")
            .Produces<AssetDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");
            
        assetGroup.MapPut("/{id:guid}", UpdateAsset)
            .WithName("UpdateAsset")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");
            
        assetGroup.MapDelete("/{id:guid}", DeleteAsset)
            .WithName("DeleteAsset")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");

        assetGroup.MapPatch("/{id:guid}/parent", MoveAsset)
            .WithName("MoveAsset")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");

        return app;
    }

    private static async Task<IResult> GetAssetHierarchy(
        [FromServices] IAssetService assetService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var hierarchy = await assetService.GetAssetHierarchyAsync(tenantId, cancellationToken);
        return hierarchy is not null ? TypedResults.Ok(hierarchy) : TypedResults.NotFound();
    }
    
    private static async Task<IResult> GetAssetById(
        Guid id,
        [FromServices] IAssetService assetService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var asset = await assetService.GetAssetByIdAsync(id, tenantId, cancellationToken);
        return asset is not null ? TypedResults.Ok(asset) : TypedResults.NotFound();
    }

    private static async Task<IResult> CreateAsset(
        [FromBody] CreateAssetRequest request,
        [FromServices] IAssetService assetService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var createdAsset = await assetService.CreateAssetAsync(tenantId, request, cancellationToken);
        var uri = linkGenerator.GetUriByName(httpContext, "GetAssetById", new { id = createdAsset.Id });
        return TypedResults.Created(uri, createdAsset);
    }
    
    private static async Task<IResult> UpdateAsset(
        Guid id,
        [FromBody] UpdateAssetRequest request,
        [FromServices] IAssetService assetService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        await assetService.UpdateAssetAsync(id, tenantId, request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteAsset(
        Guid id,
        [FromServices] IAssetService assetService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        await assetService.DeleteAssetAsync(id, tenantId, cancellationToken);
        return TypedResults.NoContent();
    }
    
    private static async Task<IResult> MoveAsset(
        Guid id,
        [FromBody] MoveAssetRequest request,
        [FromServices] IAssetService assetService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        await assetService.MoveAssetAsync(id, tenantId, request, cancellationToken);
        return TypedResults.NoContent();
    }
}