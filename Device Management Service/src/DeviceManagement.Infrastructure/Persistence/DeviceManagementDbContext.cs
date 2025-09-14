using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Services.DeviceManagement.Domain.Entities;
using System.Services.Shared.Infrastructure.Persistence; // For BaseAuditableEntity

namespace System.Services.DeviceManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the database context for the Device Management service.
    /// </summary>
    public class DeviceManagementDbContext : DbContext
    {
        public DeviceManagementDbContext(DbContextOptions<DeviceManagementDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for OPC Core Client entities.
        /// </summary>
        public DbSet<OpcCoreClient> OpcCoreClients { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Provisioning Token entities.
        /// </summary>
        public DbSet<ProvisioningToken> ProvisioningTokens { get; set; }

        /// <summary>
        /// Configures the model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is a clean way to apply all IEntityTypeConfiguration classes
            // from this assembly without having to add them one by one.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Overrides SaveChangesAsync to update audit properties automatically.
        /// This assumes a shared BaseAuditableEntity with Created/LastModified properties.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedUtc = DateTime.UtcNow;
                        entry.Entity.LastModifiedUtc = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedUtc = DateTime.UtcNow;
                        break;
                }
            }
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}