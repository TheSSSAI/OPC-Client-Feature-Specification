using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Interfaces;
using DeviceManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implements the repository for accessing and managing OpcCoreClient entities using Entity Framework Core.
/// </summary>
public class ClientRepository : IClientRepository
{
    private readonly DeviceManagementDbContext _context;

    public ClientRepository(DeviceManagementDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task AddAsync(OpcCoreClient client, CancellationToken cancellationToken = default)
    {
        await _context.OpcCoreClients.AddAsync(client, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<OpcCoreClient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OpcCoreClients.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<OpcCoreClient?> GetByCertificateCommonNameAsync(string commonName, CancellationToken cancellationToken = default)
    {
        return await _context.OpcCoreClients
            .SingleOrDefaultAsync(c => c.CertificateCommonName == commonName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<OpcCoreClient>> GetAllForTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.OpcCoreClients
            .AsNoTracking()
            .Where(c => c.TenantId == tenantId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Update(OpcCoreClient client)
    {
        // EF Core's change tracker will handle the update on SaveChangesAsync
        _context.OpcCoreClients.Update(client);
    }
    
    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}