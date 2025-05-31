using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.DTOs.Common;

namespace ProjectPipeline.Core.Interfaces.Services;

/// <summary>
/// Project service interface
/// </summary>
public interface IProjectService
{
    Task<ApiResponse<ProjectResponse>> CreateProjectAsync(CreateProjectRequest request);
    Task<ApiResponse<ProjectResponse>> UpdateProjectAsync(int id, UpdateProjectRequest request);
    Task<ApiResponse<bool>> DeleteProjectAsync(int id);
    Task<ApiResponse<ProjectResponse>> GetProjectByIdAsync(int id);
    Task<ApiResponse<PaginatedResult<ProjectResponse>>> GetProjectsAsync(int page, int pageSize);
    Task<ApiResponse<IEnumerable<ProjectResponse>>> GetProjectsByStatusAsync(string status);
    Task<ApiResponse<IEnumerable<ProjectResponse>>> GetProjectsByBusinessUnitAsync(int businessUnitId);
}
