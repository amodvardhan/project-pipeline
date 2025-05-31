using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Interfaces.Repositories;

/// <summary>
/// Project repository interface for project-specific operations
/// </summary>
public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatusEnum status);
    Task<IEnumerable<Project>> GetProjectsByBusinessUnitAsync(int businessUnitId);
    Task<IEnumerable<Project>> GetProjectsByUserAsync(string userId);
    Task<IEnumerable<Project>> GetProjectsWithDetailsAsync();
    Task<Project?> GetProjectWithDetailsAsync(int projectId);
}
