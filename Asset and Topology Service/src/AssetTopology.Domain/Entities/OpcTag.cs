using System.Shared.Domain;

namespace System.Services.AssetTopology.Domain.Entities
{
    /// <summary>
    /// Represents a data point from an OPC server that is mapped to an asset.
    /// This provides the critical link between raw industrial data and its physical context.
    /// This entity supports REQ-1-047.
    /// </summary>
    public class OpcTag : BaseEntity
    {
        /// <summary>
        /// The identifier for the tenant that owns this tag mapping. Essential for data isolation.
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// The unique identifier of the tag within the OPC server's namespace (e.g., "ns=2;s=Machine1.Temperature").
        /// </summary>
        public string NodeId { get; private set; }

        /// <summary>
        /// A user-friendly name for the tag, used for display purposes (e.g., "Motor Temperature").
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Foreign key linking this tag to its parent asset.
        /// </summary>
        public Guid AssetId { get; private set; }

        /// <summary>
        /// Navigation property to the parent asset.
        /// </summary>
        public virtual Asset Asset { get; private set; }

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private OpcTag() { }

        private OpcTag(Guid tenantId, string nodeId, string name, Guid assetId)
        {
            TenantId = tenantId;
            NodeId = nodeId;
            Name = name;
            AssetId = assetId;
        }

        /// <summary>
        /// Factory method to create a new OpcTag mapping.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant owning the mapping.</param>
        /// <param name="nodeId">The OPC server node ID for the tag.</param>
        /// <param name="name">The user-friendly name for the tag.</param>
        /// <param name="asset">The asset to which this tag is being mapped.</param>
        /// <returns>A new, valid OpcTag instance.</returns>
        /// <exception cref="ArgumentException">Thrown if tenantId or assetId is empty, or if nodeId/name are null/whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the asset is null.</exception>
        public static OpcTag Create(Guid tenantId, string nodeId, string name, Asset asset)
        {
            ArgumentNullException.ThrowIfNull(asset);

            if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }

            if (string.IsNullOrWhiteSpace(nodeId))
            {
                throw new ArgumentException("OPC Tag NodeId cannot be null or whitespace.", nameof(nodeId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("OPC Tag name cannot be null or whitespace.", nameof(name));
            }
            
            var tag = new OpcTag(tenantId, nodeId, name, asset.Id);
            // This does not add to the asset's collection. The Application service is responsible for that orchestration.
            // Example: var tag = OpcTag.Create(...); asset.AddTag(tag);
            return tag;
        }

        /// <summary>
        /// Updates the user-friendly name of the OPC tag.
        /// </summary>
        /// <param name="newName">The new name for the tag.</param>
        /// <exception cref="ArgumentException">Thrown if the new name is null or whitespace.</exception>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("OPC Tag name cannot be null or whitespace.", nameof(newName));
            }
            Name = newName;
        }
    }
}