using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetTopology.Infrastructure.Persistence.Configurations
{
    public class AssetTemplateConfiguration : IEntityTypeConfiguration<AssetTemplate>
    {
        public void Configure(EntityTypeBuilder<AssetTemplate> builder)
        {
            builder.ToTable("AssetTemplates");

            builder.HasKey(at => at.Id);

            builder.Property(at => at.TenantId)
                .IsRequired();

            builder.Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(at => at.Description)
                .HasMaxLength(1024);

            builder.Property(at => at.PropertiesJson)
                .IsRequired();

            // A template name must be unique within a tenant.
            builder.HasIndex(at => new { at.TenantId, at.Name })
                .IsUnique();
        }
    }
}