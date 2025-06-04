using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for project operations
    /// </summary>
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<Project?> GetProjectWithDetailsAsync(int projectId);
        Task<IEnumerable<Project>> GetProjectsByBusinessUnitAsync(int businessUnitId);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatusEnum status);
        Task<IEnumerable<Project>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Project>> GetProjectsByUserAsync(string userId); // Missing method
        Task<IEnumerable<Project>> GetProjectsWithDetailsAsync(); // Missing method
        Task<decimal> GetTotalProjectValueAsync();
        Task<decimal> GetTotalProjectValueByStatusAsync(ProjectStatusEnum status);
        Task<int> GetProjectCountByStatusAsync(ProjectStatusEnum status);
        Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm);
    }
}
