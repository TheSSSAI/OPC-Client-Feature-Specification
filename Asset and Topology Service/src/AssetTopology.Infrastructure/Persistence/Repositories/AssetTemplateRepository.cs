using AssetTopology.Application.Interfaces;
using AssetTopology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetTopology.Infrastructure.Persistence.Repositories;

public sealed class AssetTemplateRepository : IAssetTemplateRepository
{
    private readonly AssetTopologyDbContext _context;
    private readonly ILogger<AssetTemplateRepository> _logger;

    public AssetTemplateRepository(AssetTopologyDbContext context, ILogger<AssetTemplateRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<AssetTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AssetTemplates.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<AssetTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AssetTemplates.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string name, Guid? currentTemplateId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AssetTemplates.AsNoTracking()
            .Where(t => t.Name == name);

        if (currentTemplateId.HasValue)
        {
            query = query.Where(t => t.Id != currentTemplateId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsTemplateInUseAsync(Guid templateId, CancellationToken cancellationToken = default)
    {
        // This is a placeholder for the actual logic.
        // In a real implementation, the Asset entity would have a nullable AssetTemplateId.
        // The check would be:
        // return await _context.Assets.AnyAsync(a => a.AssetTemplateId == templateId, cancellationToken);
        _logger.LogWarning("IsTemplateInUseAsync check is a placeholder and always returns false.");
        await Task.Delay(10, cancellationToken); // Simulate async work
        return false;
    }

    public async Task AddAsync(AssetTemplate assetTemplate, CancellationToken cancellationToken = default)
    {
        await _context.AssetTemplates.AddAsync(assetTemplate, cancellationToken);
    }

    public void Update(AssetTemplate assetTemplate)
    {
        _context.AssetTemplates.Update(assetTemplate);
    }

    public void Delete(AssetTemplate assetTemplate)
    {
        _context.AssetTemplates.Remove(assetTemplate);
    }
}