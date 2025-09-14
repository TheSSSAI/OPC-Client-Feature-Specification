using Microsoft.EntityFrameworkCore;
using System.Services.IdentityAccess.Domain.Entities;
using System.Services.IdentityAccess.Domain.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace System.Services.IdentityAccess.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IamDbContext _context;

    public UserRepository(IamDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        // Note: The global query filter for TenantId is automatically applied by EF Core.
        // However, since email is globally unique in this design, we might want a way to bypass this
        // for system-level checks. For application logic, relying on the filter is correct.
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
    
    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Remove(User user)
    {
        _context.Users.Remove(user);
    }
}