using Microsoft.AspNetCore.Mvc;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjectPipeline.API.Controllers
{
    /// <summary>
    /// Authentication controller for user login, registration, and token management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// User login endpoint
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT token and user information</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
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

                var result = await _authService.LoginAsync(request);
                
                if (!result.IsSuccess)
                {
                    return Unauthorized(ApiResponse<string>.Failure(result.Message));
                }

                _logger.LogInformation("User {Email} logged in successfully", request.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Email}", request.Email);
                return StatusCode(500, ApiResponse<string>.Failure("An error occurred during login"));
            }
        }

        /// <summary>
        /// User registration endpoint
        /// </summary>
        /// <param name="request">Registration details</param>
        /// <returns>Registration result</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
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

                var result = await _authService.RegisterAsync(request);
                
                if (!result.IsSuccess)
                {
                    return BadRequest(ApiResponse<string>.Failure(result.Message));
                }

                _logger.LogInformation("User {Email} registered successfully", request.Email);
                return CreatedAtAction(nameof(Login), new { }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", request.Email);
                return StatusCode(500, ApiResponse<string>.Failure("An error occurred during registration"));
            }
        }

        /// <summary>
        /// User logout endpoint
        /// </summary>
        /// <returns>Logout result</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ApiResponse<bool>.Failure("User not found"));
                }

                var result = await _authService.LogoutAsync(userId);
                _logger.LogInformation("User {UserId} logged out", userId);
                
                return Ok(ApiResponse<bool>.Success(result, "Logged out successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, ApiResponse<bool>.Failure("An error occurred during logout"));
            }
        }

        /// <summary>
        /// Change password endpoint
        /// </summary>
        /// <param name="request">Password change request</param>
        /// <returns>Password change result</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
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

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ApiResponse<bool>.Failure("User not found"));
                }

                var result = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
                
                if (!result)
                {
                    return BadRequest(ApiResponse<bool>.Failure("Failed to change password"));
                }

                _logger.LogInformation("Password changed for user {UserId}", userId);
                return Ok(ApiResponse<bool>.Success(true, "Password changed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password change");
                return StatusCode(500, ApiResponse<bool>.Failure("An error occurred during password change"));
            }
        }

        /// <summary>
        /// Get current user information
        /// </summary>
        /// <returns>Current user details</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var name = User.FindFirst(ClaimTypes.Name)?.Value;
                var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value);

                var userResponse = new UserResponse
                {
                    Id = userId ?? "",
                    Email = email ?? "",
                    FirstName = name?.Split(' ').FirstOrDefault() ?? "",
                    LastName = name?.Split(' ').LastOrDefault() ?? ""
                };

                return Ok(ApiResponse<UserResponse>.Success(userResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                return StatusCode(500, ApiResponse<UserResponse>.Failure("An error occurred"));
            }
        }
    }
}
