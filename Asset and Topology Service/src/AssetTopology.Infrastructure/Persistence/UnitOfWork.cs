using AssetTopology.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace AssetTopology.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AssetTopologyDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(AssetTopologyDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully saved {Count} changes to the database.", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database.");
            // In a real application, you might also want to inspect DbUpdateException for constraint violations.
            throw; // Re-throw to be handled by the application layer or global exception handler.
        }
    }
}