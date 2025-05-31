using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Core.Interfaces.Services;

namespace ProjectPipeline.Infrastructure.Services;

/// <summary>
/// Project service implementation
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<ProjectResponse>> CreateProjectAsync(CreateProjectRequest request)
    {
        try
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                ClientName = request.ClientName,
                EstimatedValue = request.EstimatedValue,
                Status = ProjectStatusEnum.Pipeline,
                StartDate = request.StartDate,
                ExpectedClosureDate = request.ExpectedClosureDate,
                BusinessUnitId = request.BusinessUnitId,
                Technology = request.Technology,
                ProjectType = request.ProjectType,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            var response = MapToProjectResponse(project);
            return ApiResponse<ProjectResponse>.Success(response, "Project created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectResponse>.Failure($"Error creating project: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProjectResponse>> UpdateProjectAsync(int id, UpdateProjectRequest request)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
            {
                return ApiResponse<ProjectResponse>.Failure("Project not found");
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.ClientName = request.ClientName;
            project.EstimatedValue = request.EstimatedValue;
            project.ActualValue = request.ActualValue;
            project.Status = request.Status;
            project.StatusReason = request.StatusReason;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.ExpectedClosureDate = request.ExpectedClosureDate;
            project.Technology = request.Technology;
            project.ProjectType = request.ProjectType;
            project.UpdatedAt = DateTime.UtcNow;
            project.UpdatedBy = request.UpdatedBy;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();

            var response = MapToProjectResponse(project);
            return ApiResponse<ProjectResponse>.Success(response, "Project updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectResponse>.Failure($"Error updating project: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteProjectAsync(int id)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
            {
                return ApiResponse<bool>.Failure("Project not found");
            }

            project.IsDeleted = true;
            project.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, "Project deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Failure($"Error deleting project: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProjectResponse>> GetProjectByIdAsync(int id)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetProjectWithDetailsAsync(id);
            if (project == null)
            {
                return ApiResponse<ProjectResponse>.Failure("Project not found");
            }

            var response = MapToProjectResponse(project);
            return ApiResponse<ProjectResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectResponse>.Failure($"Error retrieving project: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PaginatedResult<ProjectResponse>>> GetProjectsAsync(int page, int pageSize)
    {
        try
        {
            var projects = await _unitOfWork.Projects.GetProjectsWithDetailsAsync();
            var totalCount = projects.Count();

            var paginatedProjects = projects
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToProjectResponse)
                .ToList();

            var result = new PaginatedResult<ProjectResponse>(paginatedProjects, totalCount, page, pageSize);
            return ApiResponse<PaginatedResult<ProjectResponse>>.Success(result);
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResult<ProjectResponse>>.Failure($"Error retrieving projects: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<ProjectResponse>>> GetProjectsByStatusAsync(string status)
    {
        try
        {
            if (!Enum.TryParse<ProjectStatusEnum>(status, true, out var statusEnum))
            {
                return ApiResponse<IEnumerable<ProjectResponse>>.Failure("Invalid status");
            }

            var projects = await _unitOfWork.Projects.GetProjectsByStatusAsync(statusEnum);
            var response = projects.Select(MapToProjectResponse);
            return ApiResponse<IEnumerable<ProjectResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ProjectResponse>>.Failure($"Error retrieving projects: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<ProjectResponse>>> GetProjectsByBusinessUnitAsync(int businessUnitId)
    {
        try
        {
            var projects = await _unitOfWork.Projects.GetProjectsByBusinessUnitAsync(businessUnitId);
            var response = projects.Select(MapToProjectResponse);
            return ApiResponse<IEnumerable<ProjectResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ProjectResponse>>.Failure($"Error retrieving projects: {ex.Message}");
        }
    }

    private static ProjectResponse MapToProjectResponse(Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            ClientName = project.ClientName,
            EstimatedValue = project.EstimatedValue,
            ActualValue = project.ActualValue,
            Status = project.Status.ToString(),
            StatusReason = project.StatusReason,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ExpectedClosureDate = project.ExpectedClosureDate,
            BusinessUnitId = project.BusinessUnitId,
            BusinessUnitName = project.BusinessUnit?.Name,
            Technology = project.Technology,
            ProjectType = project.ProjectType,
            ProfilesSubmitted = project.ProfilesSubmitted,
            ProfilesShortlisted = project.ProfilesShortlisted,
            ProfilesSelected = project.ProfilesSelected,
            CreatedAt = project.CreatedAt,
            CreatedBy = project.CreatedBy,
            CreatedByName = project.CreatedByUser != null ? $"{project.CreatedByUser.FirstName} {project.CreatedByUser.LastName}" : null
        };
    }
}
