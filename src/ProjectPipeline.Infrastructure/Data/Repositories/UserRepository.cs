using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Repositories;

/// <summary>
/// User repository implementation for user-specific operations
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.BusinessUnit)
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<IEnumerable<User>> GetUsersByBusinessUnitAsync(int businessUnitId)
    {
        return await _dbSet
            .Where(u => u.BusinessUnitId == businessUnitId && u.IsActive)
            .Include(u => u.BusinessUnit)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _dbSet
            .Where(u => u.IsActive)
            .Include(u => u.BusinessUnit)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync();
    }

    public async Task<User?> GetUserWithBusinessUnitAsync(string userId)
    {
        return await _dbSet
            .Include(u => u.BusinessUnit)
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
    }
}
