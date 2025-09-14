using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetTopology.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// EF Core configuration for the OpcTag entity.
    /// </summary>
    public class OpcTagConfiguration : IEntityTypeConfiguration<OpcTag>
    {
        public void Configure(EntityTypeBuilder<OpcTag> builder)
        {
            builder.ToTable("OpcTags");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TenantId)
                .IsRequired();
            
            builder.Property(t => t.NodeId)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.AssetId)
                .IsRequired();

            // The relationship with Asset is configured in AssetConfiguration as the principal entity.
            // This side can be left empty or configured explicitly as well.
            // builder.HasOne(t => t.Asset)
            //     .WithMany(a => a.OpcTags)
            //     .HasForeignKey(t => t.AssetId);

            // Index for efficient querying by tenant
            builder.HasIndex(t => t.TenantId);

            // Index to enforce that a single OPC Tag (identified by its NodeId)
            // can only be mapped once within a tenant, preventing duplicate mappings.
            builder.HasIndex(t => new { t.TenantId, t.NodeId })
                .IsUnique();
        }
    }
}