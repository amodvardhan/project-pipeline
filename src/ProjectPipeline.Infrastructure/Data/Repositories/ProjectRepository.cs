using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Project?> GetProjectWithDetailsAsync(int projectId)
        {
            return await _dbSet
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .Include(p => p.UpdatedByUser)
                .Include(p => p.ProfileSubmissions)
                .FirstOrDefaultAsync(p => p.Id == projectId && !p.IsDeleted);
        }

        public async Task<IEnumerable<Project>> GetProjectsByBusinessUnitAsync(int businessUnitId)
        {
            return await _dbSet
                .Where(p => p.BusinessUnitId == businessUnitId && !p.IsDeleted)
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatusEnum status)
        {
            return await _dbSet
                .Where(p => p.Status == status && !p.IsDeleted)
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && !p.IsDeleted)
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // Implementation of missing method: GetProjectsByUserAsync
        public async Task<IEnumerable<Project>> GetProjectsByUserAsync(string userId)
        {
            return await _dbSet
                .Where(p => p.CreatedBy == userId && !p.IsDeleted)
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // Implementation of missing method: GetProjectsWithDetailsAsync
        public async Task<IEnumerable<Project>> GetProjectsWithDetailsAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .Include(p => p.UpdatedByUser)
                .Include(p => p.ProfileSubmissions)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalProjectValueAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .SumAsync(p => p.EstimatedValue ?? 0);
        }

        public async Task<decimal> GetTotalProjectValueByStatusAsync(ProjectStatusEnum status)
        {
            return await _dbSet
                .Where(p => p.Status == status && !p.IsDeleted)
                .SumAsync(p => p.EstimatedValue ?? 0);
        }

        public async Task<int> GetProjectCountByStatusAsync(ProjectStatusEnum status)
        {
            return await _dbSet
                .CountAsync(p => p.Status == status && !p.IsDeleted);
        }

        public async Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted && 
                           (p.Name.Contains(searchTerm) || 
                            p.ClientName.Contains(searchTerm) || 
                            p.Technology.Contains(searchTerm)))
                .Include(p => p.BusinessUnit)
                .Include(p => p.CreatedByUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
