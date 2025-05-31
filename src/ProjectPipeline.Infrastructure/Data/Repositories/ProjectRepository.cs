using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Repositories;

/// <summary>
/// Project repository implementation for project-specific operations
/// </summary>
public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
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

    public async Task<IEnumerable<Project>> GetProjectsByBusinessUnitAsync(int businessUnitId)
    {
        return await _dbSet
            .Where(p => p.BusinessUnitId == businessUnitId && !p.IsDeleted)
            .Include(p => p.BusinessUnit)
            .Include(p => p.CreatedByUser)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserAsync(string userId)
    {
        return await _dbSet
            .Where(p => p.CreatedBy == userId && !p.IsDeleted)
            .Include(p => p.BusinessUnit)
            .Include(p => p.CreatedByUser)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetProjectsWithDetailsAsync()
    {
        return await _dbSet
            .Where(p => !p.IsDeleted)
            .Include(p => p.BusinessUnit)
            .Include(p => p.CreatedByUser)
            .Include(p => p.ProfileSubmissions)
            .Include(p => p.ResourceAllocations)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectWithDetailsAsync(int projectId)
    {
        return await _dbSet
            .Where(p => p.Id == projectId && !p.IsDeleted)
            .Include(p => p.BusinessUnit)
            .Include(p => p.CreatedByUser)
            .Include(p => p.UpdatedByUser)
            .Include(p => p.ProfileSubmissions)
            .Include(p => p.ResourceAllocations)
            .FirstOrDefaultAsync();
    }
}
