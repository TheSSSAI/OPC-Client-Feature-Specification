using System.Services.IdentityAccess.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace System.Services.IdentityAccess.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly IamDbContext _context;

    public UnitOfWork(IamDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}