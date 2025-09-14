using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetTopology.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// EF Core configuration for the Asset entity.
    /// Defines table name, primary key, properties, indexes, and relationships.
    /// </summary>
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.TenantId)
                .IsRequired();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);

            // Configure the self-referencing relationship for the hierarchy
            builder.HasOne(a => a.ParentAsset)
                .WithMany(a => a.ChildAssets)
                .HasForeignKey(a => a.ParentAssetId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes from the database side.
                                                    // Business logic in AssetService should handle recursive deletion.

            // Configure the one-to-many relationship with OpcTags
            builder.HasMany(a => a.OpcTags)
                .WithOne(t => t.Asset)
                .HasForeignKey(t => t.AssetId)
                .OnDelete(DeleteBehavior.Cascade); // If an asset is deleted, its associated tag mappings are also deleted.

            // Index for efficient querying by tenant
            builder.HasIndex(a => a.TenantId);

            // Index to enforce unique asset names under the same parent for a given tenant.
            // A null ParentAssetId represents a root-level asset.
            builder.HasIndex(a => new { a.TenantId, a.ParentAssetId, a.Name })
                .IsUnique();
        }
    }
}