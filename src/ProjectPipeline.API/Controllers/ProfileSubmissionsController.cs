using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Core.Interfaces.Services;
using System.Security.Claims;

namespace ProjectPipeline.API.Controllers
{
    /// <summary>
    /// Controller for managing profile submissions and candidate lifecycle
    /// </summary>
    [Route("api/profile-submissions")]
    [ApiController]
    [Authorize]
    public class ProfileSubmissionsController : ControllerBase
    {
        private readonly IProfileSubmissionService _profileSubmissionService;
        private readonly ILogger<ProfileSubmissionsController> _logger;

        public ProfileSubmissionsController(
            IProfileSubmissionService profileSubmissionService, 
            ILogger<ProfileSubmissionsController> logger)
        {
            _profileSubmissionService = profileSubmissionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all profile submissions for a project
        /// </summary>
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProfileSubmissionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfilesByProject(int projectId)
        {
            try
            {
                var result = await _profileSubmissionService.GetProfilesByProjectAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profiles for project {ProjectId}", projectId);
                return StatusCode(500, ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Failure("An error occurred while retrieving profiles"));
            }
        }

        /// <summary>
        /// Create a new profile submission
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "AccountManager,DeliveryManager,DeliveryDirector,SystemAdmin")]
        [ProducesResponseType(typeof(ApiResponse<ProfileSubmissionResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileSubmissionRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<string>.Failure("Invalid input", errors));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<string>.Failure("User not found"));
            }

            var result = await _profileSubmissionService.CreateProfileSubmissionAsync(request, userId);
            
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProfile), new { id = result.Data!.Id }, result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Update profile status
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfileStatus(int id, [FromBody] UpdateProfileStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<string>.Failure("Invalid input", errors));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<string>.Failure("User not found"));
            }

            var result = await _profileSubmissionService.UpdateProfileStatusAsync(id, request, userId);
            
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return result.Message.Contains("not found") ? NotFound(result) : BadRequest(result);
        }

        /// <summary>
        /// Get profile details with complete history
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProfileSubmissionDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(int id)
        {
            var result = await _profileSubmissionService.GetProfileDetailsAsync(id);
            
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        /// <summary>
        /// Get profiles by status
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProfileSubmissionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfilesByStatus(string status)
        {
            if (!Enum.TryParse<ProfileStatusEnum>(status, true, out var statusEnum))
            {
                return BadRequest(ApiResponse<string>.Failure("Invalid status"));
            }

            var result = await _profileSubmissionService.GetProfilesByStatusAsync(statusEnum);
            return Ok(result);
        }

        /// <summary>
        /// Get current user's profile submissions
        /// </summary>
        [HttpGet("my-submissions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProfileSubmissionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMySubmissions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<string>.Failure("User not found"));
            }

            var result = await _profileSubmissionService.GetMySubmissionsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Delete a profile submission
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "DeliveryDirector,SystemAdmin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<string>.Failure("User not found"));
            }

            var result = await _profileSubmissionService.DeleteProfileSubmissionAsync(id, userId);
            
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        /// <summary>
        /// Get profile analytics for a project
        /// </summary>
        [HttpGet("analytics/{projectId}")]
        [ProducesResponseType(typeof(ApiResponse<ProfileAnalyticsResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfileAnalytics(int projectId)
        {
            var result = await _profileSubmissionService.GetProfileAnalyticsAsync(projectId);
            return Ok(result);
        }
    }
}
