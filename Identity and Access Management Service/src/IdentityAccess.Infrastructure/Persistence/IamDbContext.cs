using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Services.IdentityAccess.Domain.Entities;
using System.Services.IdentityAccess.Domain.Interfaces;
using System.Services.IdentityAccess.Infrastructure.Persistence.Interceptors;

namespace System.Services.IdentityAccess.Infrastructure.Persistence;

public class IamDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly ITenantProvider _tenantProvider;

    public IamDbContext(
        DbContextOptions<IamDbContext> options,
        ITenantProvider tenantProvider,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _tenantProvider = tenantProvider;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<License> Licenses => Set<License>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply multi-tenancy global query filters
        var tenantId = _tenantProvider.GetTenantId();

        // Note: Tenant entity itself is not filtered by tenantId.
        modelBuilder.Entity<User>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<Role>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<License>().HasQueryFilter(e => e.TenantId == tenantId);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}