using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for user operations
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _dbSet
                .Include(u => u.BusinessUnit)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithBusinessUnitAsync(string userId)
        {
            return await _dbSet
                .Include(u => u.BusinessUnit)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.BusinessUnit)
                .FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task<bool> IsEmailUniqueAsync(string email, string? excludeUserId = null)
        {
            var query = _dbSet.Where(u => u.Email != null && u.Email == email); // Fixed null reference
            
            if (!string.IsNullOrEmpty(excludeUserId))
            {
                query = query.Where(u => u.Id != excludeUserId);
            }

            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await _dbSet
                .Where(u => u.IsActive && 
                           (u.FirstName.Contains(searchTerm) || 
                            u.LastName.Contains(searchTerm) || 
                            (u.Email != null && u.Email.Contains(searchTerm)))) // Fixed null reference
                .Include(u => u.BusinessUnit)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }
    }
}
