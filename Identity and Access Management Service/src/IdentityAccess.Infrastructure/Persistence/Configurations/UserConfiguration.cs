using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Services.IdentityAccess.Domain.Entities;
using System.Services.IdentityAccess.Infrastructure.Security.Encryption;

namespace System.Services.IdentityAccess.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private readonly IEncryptionProvider _encryptionProvider;

    // In a real DI scenario, the provider would be injected. For configuration,
    // we might need to instantiate it or pass it from the DbContext.
    // For simplicity, we'll new it up here, assuming it has a parameterless constructor.
    public UserConfiguration()
    {
        // This is a simplification. A real implementation would resolve this via DI
        // if the configuration registration process supports it, or pass it in.
        // Let's assume a static provider or a simple one for this example.
        _encryptionProvider = new AesEncryptionProvider(new EncryptionOptions { Key = "DefaultKey-MustBeOverwrittenBySecrets-32Bytes" });
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(u => u.TenantId)
            .IsRequired()
            .HasColumnName("tenant_id");

        var encryptedStringConverter = new EncryptedStringConverter(_encryptionProvider);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("first_name")
            .HasConversion(encryptedStringConverter);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("last_name")
            .HasConversion(encryptedStringConverter);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("email")
            .HasConversion(encryptedStringConverter);

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("is_active");

        builder.Property(u => u.Created)
            .HasColumnName("created_at");

        builder.Property(u => u.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(u => u.LastModified)
            .HasColumnName("last_modified_at");

        builder.Property(u => u.LastModifiedBy)
            .HasColumnName("last_modified_by");

        // Relationships
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(u => u.Email).IsUnique(); // Email must be unique across all tenants in this model
        builder.HasIndex(u => u.TenantId);
    }
}