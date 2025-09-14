using AssetTopology.Application.Interfaces;
using AssetTopology.Contracts.OpcTags;
using Microsoft.AspNetCore.Mvc;
using AssetTopology.API.Extensions;

namespace AssetTopology.API.Endpoints;

public static class TagMappingEndpoints
{
    public static IEndpointRouteBuilder MapTagMappingEndpoints(this IEndpointRouteBuilder app)
    {
        var mappingGroup = app.MapGroup("/api/v1/assets/{assetId:guid}/tags").WithTags("Tag Mappings");

        mappingGroup.MapGet("/", GetTagsForAsset)
            .WithName("GetTagsForAsset")
            .Produces<IEnumerable<OpcTagDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");

        mappingGroup.MapPost("/", MapTagToAsset)
            .WithName("MapTagToAsset")
            .Produces<OpcTagDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");

        mappingGroup.MapDelete("/{tagId:guid}", UnmapTagFromAsset)
            .WithName("UnmapTagFromAsset")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");

        return app;
    }

    private static async Task<IResult> GetTagsForAsset(
        Guid assetId,
        [FromServices] ITagMappingService mappingService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var tags = await mappingService.GetTagsForAssetAsync(assetId, tenantId, cancellationToken);
        return TypedResults.Ok(tags);
    }

    private static async Task<IResult> MapTagToAsset(
        Guid assetId,
        [FromBody] MapTagRequest request,
        [FromServices] ITagMappingService mappingService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var mappedTag = await mappingService.MapTagToAssetAsync(assetId, tenantId, request, cancellationToken);
        // Note: The location URI for a created sub-resource can be complex. 
        // For simplicity, we return the created object without a Location header.
        return TypedResults.Created($"/api/v1/assets/{assetId}/tags/{mappedTag.Id}", mappedTag);
    }

    private static async Task<IResult> UnmapTagFromAsset(
        Guid assetId,
        Guid tagId,
        [FromServices] ITagMappingService mappingService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        await mappingService.UnmapTagFromAssetAsync(assetId, tagId, tenantId, cancellationToken);
        return TypedResults.NoContent();
    }
}