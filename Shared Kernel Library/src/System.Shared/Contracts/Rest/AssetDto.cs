using System.Text.Json.Serialization;

namespace System.Shared.Contracts.Rest;

/// <summary>
/// Represents an asset in the asset hierarchy for data transfer purposes in REST APIs.
/// This DTO is used for creating, retrieving, and updating asset information.
/// </summary>
/// <param name="Id">The unique identifier of the asset.</param>
/// <param name="Name">The name of the asset.</param>
/// <param name="ParentId">The unique identifier of the parent asset. Null for root assets.</param>
/// <param name="Children">A list of child assets, used for representing hierarchical structures.</param>
public record AssetDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("parentId")] Guid? ParentId,
    [property: JsonPropertyName("children")] List<AssetDto>? Children = null
);