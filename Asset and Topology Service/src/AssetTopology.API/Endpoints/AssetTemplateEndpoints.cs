using AssetTopology.Application.Interfaces;
using AssetTopology.Contracts.AssetTemplates;
using Microsoft.AspNetCore.Mvc;
using AssetTopology.API.Extensions;

namespace AssetTopology.API.Endpoints;

public static class AssetTemplateEndpoints
{
    public static IEndpointRouteBuilder MapAssetTemplateEndpoints(this IEndpointRouteBuilder app)
    {
        var templateGroup = app.MapGroup("/api/v1/asset-templates").WithTags("Asset Templates");

        templateGroup.MapGet("/", GetAllTemplates)
            .WithName("GetAllTemplates")
            .Produces<IEnumerable<AssetTemplateDto>>()
            .RequireAuthorization("AdminOrEngineer");
            
        templateGroup.MapGet("/{id:guid}", GetTemplateById)
            .WithName("GetTemplateById")
            .Produces<AssetTemplateDto>()
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");

        templateGroup.MapPost("/", CreateTemplate)
            .WithName("CreateTemplate")
            .Produces<AssetTemplateDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");

        templateGroup.MapPut("/{id:guid}", UpdateTemplate)
            .WithName("UpdateTemplate")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");

        templateGroup.MapDelete("/{id:guid}", DeleteTemplate)
            .WithName("DeleteTemplate")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOrEngineer");
            
        templateGroup.MapPost("/{id:guid}/instantiate", InstantiateTemplate)
            .WithName("InstantiateTemplate")
            .Produces<AssetTopology.Contracts.Assets.AssetDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOrEngineer");

        return app;
    }

    private static async Task<IResult> GetAllTemplates(
        [FromServices] IAssetTemplateService templateService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var templates = await templateService.GetAllAsync(tenantId, cancellationToken);
        return TypedResults.Ok(templates);
    }
    
    private static async Task<IResult> GetTemplateById(
        Guid id,
        [FromServices] IAssetTemplateService templateService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var template = await templateService.GetByIdAsync(id, tenantId, cancellationToken);
        return template is not null ? TypedResults.Ok(template) : TypedResults.NotFound();
    }

    private static async Task<IResult> CreateTemplate(
        [FromBody] CreateAssetTemplateRequest request,
        [FromServices] IAssetTemplateService templateService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var createdTemplate = await templateService.CreateAsync(tenantId, request, cancellationToken);
        var uri = linkGenerator.GetUriByName(httpContext, "GetTemplateById", new { id = createdTemplate.Id });
        return TypedResults.Created(uri, createdTemplate);
    }
    
    private static async Task<IResult> UpdateTemplate(
        Guid id,
        [FromBody] UpdateAssetTemplateRequest request,
        [FromServices] IAssetTemplateService templateService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        await templateService.UpdateAsync(id, tenantId, request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteTemplate(
        Guid id,
        [FromServices] IAssetTemplateService templateService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        await templateService.DeleteAsync(id, tenantId, cancellationToken);
        return TypedResults.NoContent();
    }
    
    private static async Task<IResult> InstantiateTemplate(
        Guid id,
        [FromBody] InstantiateTemplateRequest request,
        [FromServices] IAssetTemplateService templateService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var tenantId = httpContext.User.GetTenantId();
        var createdAsset = await templateService.InstantiateAsync(id, tenantId, request, cancellationToken);
        var uri = linkGenerator.GetUriByName(httpContext, "GetAssetById", new { id = createdAsset.Id });
        return TypedResults.Created(uri, createdAsset);
    }
}