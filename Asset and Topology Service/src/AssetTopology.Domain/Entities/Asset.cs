using System.Shared.Domain;

namespace System.Services.AssetTopology.Domain.Entities
{
    /// <summary>
    /// Represents a node in the ISA-95 asset hierarchy (e.g., Site, Area, Line, Machine).
    /// This is the Aggregate Root for the Asset and OpcTag entities.
    /// It encapsulates the business logic for maintaining the integrity of the hierarchy.
    /// </summary>
    public class Asset : BaseEntity
    {
        private readonly HashSet<Asset> _childAssets = new();
        private readonly HashSet<OpcTag> _opcTags = new();

        /// <summary>
        /// Identifier for the tenant that owns this asset. Crucial for data isolation.
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// Human-readable name of the asset (e.g., "Production Line 1"). Must be unique among its siblings.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Foreign key to the parent asset. A null value indicates a root-level asset.
        /// </summary>
        public Guid? ParentAssetId { get; private set; }

        /// <summary>
        /// Navigation property to the parent asset.
        /// </summary>
        public virtual Asset? ParentAsset { get; private set; }

        /// <summary>
        /// Navigation property to the collection of child assets.
        /// </summary>
        public virtual IReadOnlyCollection<Asset> ChildAssets => _childAssets;

        /// <summary>
        /// Navigation property to the collection of OPC tags mapped to this asset.
        /// </summary>
        public virtual IReadOnlyCollection<OpcTag> OpcTags => _opcTags;

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private Asset() { }

        private Asset(Guid tenantId, string name, Asset? parentAsset = null)
        {
            TenantId = tenantId;
            Name = name;
            ParentAsset = parentAsset;
            ParentAssetId = parentAsset?.Id;
        }

        /// <summary>
        /// Factory method to create a new Asset. Ensures all business rules for creation are met.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant owning the asset.</param>
        /// <param name="name">The name of the asset.</param>
        /// <param name="parentAsset">The optional parent asset.</param>
        /// <returns>A new, valid Asset instance.</returns>
        /// <exception cref="ArgumentException">Thrown if tenantId is empty or name is null/whitespace.</exception>
        public static Asset Create(Guid tenantId, string name, Asset? parentAsset = null)
        {
            if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Asset name cannot be null or whitespace.", nameof(name));
            }

            return new Asset(tenantId, name, parentAsset);
        }

        /// <summary>
        /// Updates the name of the asset.
        /// </summary>
        /// <param name="newName">The new name for the asset.</param>
        /// <exception cref="ArgumentException">Thrown if the new name is null or whitespace.</exception>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Asset name cannot be null or whitespace.", nameof(newName));
            }
            Name = newName;
        }

        /// <summary>
        /// Sets or updates the parent of this asset, enforcing hierarchy rules.
        /// </summary>
        /// <param name="newParent">The new parent asset, or null to make this a root asset.</param>
        /// <exception cref="InvalidOperationException">Thrown if an attempt is made to set the asset as its own parent.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a circular dependency is detected.</exception>
        public void SetParent(Asset? newParent)
        {
            if (newParent != null && newParent.Id == Id)
            {
                throw new InvalidOperationException("An asset cannot be its own parent.");
            }

            if (newParent != null)
            {
                // Check for circular dependency by traversing up from the new parent
                var current = newParent;
                while (current != null)
                {
                    if (current.Id == Id)
                    {
                        throw new InvalidOperationException("Circular dependency detected. An asset cannot be a descendant of itself.");
                    }
                    current = current.ParentAsset;
                }
            }

            ParentAsset = newParent;
            ParentAssetId = newParent?.Id;
        }

        /// <summary>
        /// Adds an OPC tag mapping to this asset.
        /// </summary>
        /// <param name="tag">The OPC tag to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the tag is null.</exception>
        public void AddTag(OpcTag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);
            _opcTags.Add(tag);
        }

        /// <summary>
        /// Removes an OPC tag mapping from this asset by its ID.
        /// </summary>
        /// <param name="tagId">The ID of the OPC tag to remove.</param>
        /// <returns>True if the tag was found and removed, otherwise false.</returns>
        public bool RemoveTag(Guid tagId)
        {
            var tagToRemove = _opcTags.FirstOrDefault(t => t.Id == tagId);
            return tagToRemove != null && _opcTags.Remove(tagToRemove);
        }
    }
}