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
    /// Users controller for managing user accounts and profiles
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paginated list of users</returns>
        [HttpGet]
        [Authorize(Roles = "SystemAdmin,BusinessUnitHead,DeliveryDirector")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<UserResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _userService.GetUsersAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, ApiResponse<PaginatedResult<UserResponse>>.Failure("An error occurred while retrieving users"));
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid user ID"));
                }

                // Users can only view their own profile unless they have admin privileges
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var isAdmin = User.IsInRole("SystemAdmin") || User.IsInRole("BusinessUnitHead") || User.IsInRole("DeliveryDirector");
                
                if (currentUserId != id && !isAdmin)
                {
                    return Forbid();
                }

                var result = await _userService.GetUserByIdAsync(id);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user {UserId}", id);
                return StatusCode(500, ApiResponse<UserResponse>.Failure("An error occurred while retrieving the user"));
            }
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">User update request</param>
        /// <returns>Updated user details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid user ID"));
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return BadRequest(ApiResponse<string>.Failure("Invalid input", errors));
                }

                // Users can only update their own profile unless they have admin privileges
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var isAdmin = User.IsInRole("SystemAdmin") || User.IsInRole("BusinessUnitHead");
                
                if (currentUserId != id && !isAdmin)
                {
                    return Forbid();
                }

                var result = await _userService.UpdateUserAsync(id, request);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                _logger.LogInformation("User {UserId} updated", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, ApiResponse<UserResponse>.Failure("An error occurred while updating the user"));
            }
        }

        /// <summary>
        /// Deactivate user account
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Deactivation result</returns>
        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "SystemAdmin,BusinessUnitHead")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid user ID"));
                }

                var result = await _userService.DeactivateUserAsync(id);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("User {UserId} deactivated by {CurrentUserId}", id, currentUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user {UserId}", id);
                return StatusCode(500, ApiResponse<bool>.Failure("An error occurred while deactivating the user"));
            }
        }

        /// <summary>
        /// Activate user account
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Activation result</returns>
        [HttpPost("{id}/activate")]
        [Authorize(Roles = "SystemAdmin,BusinessUnitHead")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid user ID"));
                }

                var result = await _userService.ActivateUserAsync(id);
                
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("User {UserId} activated by {CurrentUserId}", id, currentUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user {UserId}", id);
                return StatusCode(500, ApiResponse<bool>.Failure("An error occurred while activating the user"));
            }
        }

        /// <summary>
        /// Get users by business unit
        /// </summary>
        /// <param name="businessUnitId">Business unit ID</param>
        /// <returns>List of users in the specified business unit</returns>
        [HttpGet("business-unit/{businessUnitId}")]
        [Authorize(Roles = "SystemAdmin,BusinessUnitHead,DeliveryDirector")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersByBusinessUnit(int businessUnitId)
        {
            try
            {
                if (businessUnitId <= 0)
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid business unit ID"));
                }

                var result = await _userService.GetUsersByBusinessUnitAsync(businessUnitId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users for business unit {BusinessUnitId}", businessUnitId);
                return StatusCode(500, ApiResponse<IEnumerable<UserResponse>>.Failure("An error occurred while retrieving users"));
            }
        }
    }
}
