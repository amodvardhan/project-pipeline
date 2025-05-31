using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Interfaces.Services;
using System.Security.Claims;

namespace ProjectPipeline.API.Controllers
{
    /// <summary>
    /// Projects controller for managing project pipeline data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        /// <summary>
        /// Get all projects with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paginated list of projects</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ProjectResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjects([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _projectService.GetProjectsAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects");
                return StatusCode(500, ApiResponse<PaginatedResult<ProjectResponse>>.Failure("An error occurred while retrieving projects"));
            }
        }

        /// <summary>
        /// Get project by ID
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Project details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid project ID"));
                }

                var result = await _projectService.GetProjectByIdAsync(id);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project {ProjectId}", id);
                return StatusCode(500, ApiResponse<ProjectResponse>.Failure("An error occurred while retrieving the project"));
            }
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="request">Project creation request</param>
        /// <returns>Created project details</returns>
        [HttpPost]
        [Authorize(Roles = "AccountManager,DeliveryManager,DeliveryDirector,SystemAdmin")]
        [ProducesResponseType(typeof(ApiResponse<ProjectResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return BadRequest(ApiResponse<string>.Failure("Invalid input", errors));
                }

                // Set the created by user ID from the current user
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ApiResponse<string>.Failure("User not found"));
                }

                request.CreatedBy = userId;

                var result = await _projectService.CreateProjectAsync(request);
                
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                _logger.LogInformation("Project {ProjectName} created by user {UserId}", request.Name, userId);
                return CreatedAtAction(nameof(GetProject), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                return StatusCode(500, ApiResponse<ProjectResponse>.Failure("An error occurred while creating the project"));
            }
        }

        /// <summary>
        /// Update an existing project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="request">Project update request</param>
        /// <returns>Updated project details</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "AccountManager,DeliveryManager,DeliveryDirector,SystemAdmin")]
        [ProducesResponseType(typeof(ApiResponse<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid project ID"));
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return BadRequest(ApiResponse<string>.Failure("Invalid input", errors));
                }

                // Set the updated by user ID from the current user
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ApiResponse<string>.Failure("User not found"));
                }

                request.UpdatedBy = userId;

                var result = await _projectService.UpdateProjectAsync(id, request);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                _logger.LogInformation("Project {ProjectId} updated by user {UserId}", id, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId}", id);
                return StatusCode(500, ApiResponse<ProjectResponse>.Failure("An error occurred while updating the project"));
            }
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "DeliveryDirector,SystemAdmin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid project ID"));
                }

                var result = await _projectService.DeleteProjectAsync(id);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("Project {ProjectId} deleted by user {UserId}", id, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", id);
                return StatusCode(500, ApiResponse<bool>.Failure("An error occurred while deleting the project"));
            }
        }

        /// <summary>
        /// Get projects by status
        /// </summary>
        /// <param name="status">Project status (Pipeline, Won, Lost, Missed)</param>
        /// <returns>List of projects with the specified status</returns>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProjectResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectsByStatus(string status)
        {
            try
            {
                var result = await _projectService.GetProjectsByStatusAsync(status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects by status {Status}", status);
                return StatusCode(500, ApiResponse<IEnumerable<ProjectResponse>>.Failure("An error occurred while retrieving projects"));
            }
        }

        /// <summary>
        /// Get projects by business unit
        /// </summary>
        /// <param name="businessUnitId">Business unit ID</param>
        /// <returns>List of projects for the specified business unit</returns>
        [HttpGet("business-unit/{businessUnitId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProjectResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectsByBusinessUnit(int businessUnitId)
        {
            try
            {
                if (businessUnitId <= 0)
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid business unit ID"));
                }

                var result = await _projectService.GetProjectsByBusinessUnitAsync(businessUnitId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects for business unit {BusinessUnitId}", businessUnitId);
                return StatusCode(500, ApiResponse<IEnumerable<ProjectResponse>>.Failure("An error occurred while retrieving projects"));
            }
        }
    }
}
