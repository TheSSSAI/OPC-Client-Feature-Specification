using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Interfaces;
using DeviceManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implements the repository for accessing and managing ProvisioningToken entities using Entity Framework Core.
/// </summary>
public class ProvisioningTokenRepository : IProvisioningTokenRepository
{
    private readonly DeviceManagementDbContext _context;

    public ProvisioningTokenRepository(DeviceManagementDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task AddAsync(ProvisioningToken token, CancellationToken cancellationToken = default)
    {
        await _context.ProvisioningTokens.AddAsync(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProvisioningToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        // Using FindAsync since TokenHash is the primary key. This is the most efficient lookup.
        return await _context.ProvisioningTokens.FindAsync(new object[] { tokenHash }, cancellationToken);
    }
    
    /// <inheritdoc />
    public void Update(ProvisioningToken token)
    {
        _context.ProvisioningTokens.Update(token);
    }
    
    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}