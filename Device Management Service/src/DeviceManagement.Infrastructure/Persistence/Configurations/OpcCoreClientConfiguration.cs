using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeviceManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the OpcCoreClient entity using Entity Framework Core's Fluent API.
    /// This class is discovered and applied by the DbContext in its OnModelCreating method.
    /// </summary>
    public class OpcCoreClientConfiguration : IEntityTypeConfiguration<OpcCoreClient>
    {
        public void Configure(EntityTypeBuilder<OpcCoreClient> builder)
        {
            builder.ToTable("OpcCoreClients");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            // TenantId is a critical column for data isolation and should be indexed for performance.
            builder.Property(c => c.TenantId)
                .IsRequired();
            builder.HasIndex(c => c.TenantId)
                .HasDatabaseName("IX_OpcCoreClients_TenantId");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Store the enum as a string in the database for readability.
            builder.Property(c => c.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<ClientStatus>());

            builder.Property(c => c.LastSeenUtc)
                .IsRequired(false);

            builder.Property(c => c.SoftwareVersion)
                .HasMaxLength(50);
                
            // The configuration is a large JSON blob.
            builder.Property(c => c.ConfigurationJson)
                .HasColumnType("jsonb");

            // Certificate Common Name must be unique across all clients as it's used for identification
            // in the mTLS handshake with the MQTT broker.
            builder.Property(c => c.CertificateCommonName)
                .HasMaxLength(256);
            builder.HasIndex(c => c.CertificateCommonName)
                .IsUnique()
                .HasDatabaseName("IX_OpcCoreClients_CertificateCommonName");

            // Configure properties from BaseAuditableEntity
            builder.Property(c => c.Created)
                .IsRequired();

            builder.Property(c => c.CreatedBy)
                .HasMaxLength(255);

            builder.Property(c => c.LastModified)
                .IsRequired(false);

            builder.Property(c => c.LastModifiedBy)
                .HasMaxLength(255);
        }
    }
}