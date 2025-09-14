using AssetTopology.Application.Interfaces;
using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetTopology.Infrastructure.Persistence.Repositories;

public sealed class OpcTagRepository : IOpcTagRepository
{
    private readonly AssetTopologyDbContext _context;

    public OpcTagRepository(AssetTopologyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<OpcTag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OpcTags.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }
    
    public async Task<IEnumerable<OpcTag>> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        return await _context.OpcTags
            .Where(t => t.AssetId == assetId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNodeIdUniqueAsync(string nodeId, Guid? currentTagId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.OpcTags.AsNoTracking()
            .Where(t => t.NodeId == nodeId);

        if (currentTagId.HasValue)
        {
            query = query.Where(t => t.Id != currentTagId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(OpcTag opcTag, CancellationToken cancellationToken = default)
    {
        await _context.OpcTags.AddAsync(opcTag, cancellationToken);
    }

    public void Update(OpcTag opcTag)
    {
        _context.OpcTags.Update(opcTag);
    }

    public void Delete(OpcTag opcTag)
    {
        _context.OpcTags.Remove(opcTag);
    }
}