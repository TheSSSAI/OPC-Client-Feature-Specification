using AssetTopology.Application.Interfaces;
using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Text;

namespace AssetTopology.Infrastructure.Persistence.Repositories;

public sealed class AssetRepository : IAssetRepository
{
    private readonly AssetTopologyDbContext _context;
    private readonly ILogger<AssetRepository> _logger;

    public AssetRepository(AssetTopologyDbContext context, ILogger<AssetRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Assets.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Assets.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetHierarchyByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching asset hierarchy for tenant {TenantId} using recursive CTE.", tenantId);

        // This query uses a Recursive Common Table Expression (CTE) which is the most efficient way
        // to query hierarchical data in PostgreSQL. This is critical for performance as per REQ-1-074.
        var sql = new StringBuilder(@"
            WITH RECURSIVE AssetHierarchy AS (
                -- Anchor member: select the root assets (those with no parent) for the tenant
                SELECT 
                    a.""Id"", 
                    a.""Name"", 
                    a.""ParentAssetId"", 
                    a.""TenantId"",
                    a.""CreatedAt"",
                    a.""UpdatedAt"",
                    0 as ""Level""
                FROM ""Assets"" a
                WHERE a.""ParentAssetId"" IS NULL AND a.""TenantId"" = @tenantId

                UNION ALL

                -- Recursive member: select child assets
                SELECT 
                    c.""Id"", 
                    c.""Name"", 
                    c.""ParentAssetId"", 
                    c.""TenantId"",
                    c.""CreatedAt"",
                    c.""UpdatedAt"",
                    h.""Level"" + 1
                FROM ""Assets"" c
                JOIN AssetHierarchy h ON c.""ParentAssetId"" = h.""Id""
            )
            SELECT 
                ""Id"", 
                ""Name"", 
                ""ParentAssetId"", 
                ""TenantId"",
                ""CreatedAt"",
                ""UpdatedAt""
            FROM AssetHierarchy;");

        var tenantIdParam = new NpgsqlParameter("@tenantId", tenantId);
        
        try
        {
            return await _context.Assets
                .FromSqlRaw(sql.ToString(), tenantIdParam)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve asset hierarchy for tenant {TenantId}.", tenantId);
            throw; // Re-throw to be handled by global exception handler
        }
    }

    public async Task<bool> IsNameUniqueUnderParentAsync(Guid? parentAssetId, string name, Guid? currentAssetId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Assets.AsNoTracking()
            .Where(a => a.ParentAssetId == parentAssetId && a.Name == name);
        
        if (currentAssetId.HasValue)
        {
            // When updating, exclude the current asset from the check
            query = query.Where(a => a.Id != currentAssetId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }
    
    public async Task<bool> CheckForCircularDependencyAsync(Guid assetId, Guid? newParentId, CancellationToken cancellationToken)
    {
        if (!newParentId.HasValue)
        {
            return false; // Cannot have a circular dependency with a root asset
        }

        var currentParentId = newParentId.Value;
        while (true)
        {
            if (currentParentId == assetId)
            {
                return true; // Circular dependency detected
            }

            var parent = await _context.Assets
                .AsNoTracking()
                .Where(a => a.Id == currentParentId)
                .Select(a => new { a.ParentAssetId })
                .FirstOrDefaultAsync(cancellationToken);

            if (parent == null || !parent.ParentAssetId.HasValue)
            {
                return false; // Reached the root of the tree
            }
            
            currentParentId = parent.ParentAssetId.Value;
        }
    }

    public async Task AddAsync(Asset asset, CancellationToken cancellationToken = default)
    {
        await _context.Assets.AddAsync(asset, cancellationToken);
    }

    public void Update(Asset asset)
    {
        _context.Assets.Update(asset);
    }

    public void Delete(Asset asset)
    {
        _context.Assets.Remove(asset);
    }
}