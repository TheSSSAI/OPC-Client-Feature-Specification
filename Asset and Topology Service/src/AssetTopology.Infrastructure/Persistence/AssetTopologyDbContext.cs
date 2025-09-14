using System.Reflection;
using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetTopology.Infrastructure.Persistence;

public class AssetTopologyDbContext : DbContext
{
    public AssetTopologyDbContext(DbContextOptions<AssetTopologyDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Asset> Assets { get; set; }
    public DbSet<AssetTemplate> AssetTemplates { get; set; }
    public DbSet<OpcTag> OpcTags { get; set; }
    
    // In a real multi-tenant application, tenantId would be injected via a scoped service
    // that resolves the tenant from the current user's context (e.g., JWT claim).
    // For this example, we'll assume it's set during DbContext initialization.
    // private readonly Guid? _tenantId; 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // This dynamically applies all configurations defined in the Infrastructure assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // CRITICAL: Enforce multi-tenancy at the data access layer using global query filters.
        // This prevents developers from accidentally querying data from another tenant.
        // This is a cornerstone of the data isolation strategy required by REQ-1-025.
        // In a real implementation, a more robust tenant resolution mechanism would be used.
        // For example, injecting an ITenantProvider service into the DbContext constructor.
        
        // Example for Asset entity
        modelBuilder.Entity<Asset>().HasQueryFilter(e => EF.Property<Guid>(e, "TenantId") == GetTenantId());
        
        // Example for AssetTemplate entity
        modelBuilder.Entity<AssetTemplate>().HasQueryFilter(e => EF.Property<Guid>(e, "TenantId") == GetTenantId());

        // Example for OpcTag entity
        modelBuilder.Entity<OpcTag>().HasQueryFilter(e => EF.Property<Guid>(e, "TenantId") == GetTenantId());
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetTenantIdOnAddedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetTenantIdOnAddedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    private void SetTenantIdOnAddedEntities()
    {
        var addedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added && e.Metadata.FindProperty("TenantId") != null);

        var tenantId = GetTenantId();
        
        foreach (var entry in addedEntities)
        {
            entry.Property("TenantId").CurrentValue = tenantId;
        }
    }

    // This method would be replaced by a real tenant provider service.
    // It's a placeholder to make the query filter logic compilable and clear.
    private Guid GetTenantId()
    {
        // In a real application, this would come from an injected service like:
        // return _tenantProvider.GetTenantId();
        // For demonstration, we'll use a hardcoded Guid. This would fail in a real multi-tenant scenario
        // but shows the principle of how the filter is applied.
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}