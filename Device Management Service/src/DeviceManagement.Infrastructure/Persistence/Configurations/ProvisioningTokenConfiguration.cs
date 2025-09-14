using DeviceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the ProvisioningToken entity using Entity Framework Core's Fluent API.
    /// </summary>
    public class ProvisioningTokenConfiguration : IEntityTypeConfiguration<ProvisioningToken>
    {
        public void Configure(EntityTypeBuilder<ProvisioningToken> builder)
        {
            builder.ToTable("ProvisioningTokens");

            // The TokenHash is the primary key as it's the main lookup value and must be unique.
            builder.HasKey(t => t.TokenHash);

            builder.Property(t => t.TokenHash)
                .HasMaxLength(128); // SHA256 hash is 64 hex chars, but allow for future algorithms.

            builder.Property(t => t.ClientId)
                .IsRequired();

            // Define the one-to-one relationship with OpcCoreClient.
            // A token is for one client, and a client has one (initial) token.
            // The provisioning process creates the client first, then the token, so the relationship is established.
            builder.HasOne<OpcCoreClient>()
                .WithOne()
                .HasForeignKey<ProvisioningToken>(t => t.ClientId)
                .OnDelete(DeleteBehavior.Cascade); // If the client record is deleted, the token should be too.

            builder.Property(t => t.ExpiryUtc)
                .IsRequired();

            builder.Property(t => t.IsUsed)
                .IsRequired()
                .HasDefaultValue(false);

            // Index on ExpiryUtc to allow for efficient cleanup of expired tokens by a background job.
            builder.HasIndex(t => t.ExpiryUtc)
                .HasDatabaseName("IX_ProvisioningTokens_ExpiryUtc");
        }
    }
}