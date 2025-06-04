using Microsoft.Extensions.Logging;
using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Core.Interfaces.Services;

namespace ProjectPipeline.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for user business logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<UserResponse>> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(userId);
                
                if (user == null)
                {
                    return ApiResponse<UserResponse>.Failure("User not found");
                }

                var response = MapToUserResponse(user);
                return ApiResponse<UserResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user {UserId}", userId);
                return ApiResponse<UserResponse>.Failure("An error occurred while retrieving the user");
            }
        }

        public async Task<ApiResponse<UserResponse>> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(userId);
                
                if (user == null)
                {
                    return ApiResponse<UserResponse>.Failure("User not found");
                }

                // Update user properties
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Department = request.Department;
                user.Designation = request.Designation;
                user.PhoneNumber = request.PhoneNumber;
                user.BusinessUnitId = request.BusinessUnitId;
                user.UpdatedAt = DateTime.UtcNow;
                // Removed user.UpdatedBy as it doesn't exist in User entity

                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                var response = MapToUserResponse(user);
                _logger.LogInformation("User {UserId} updated successfully", userId);

                return ApiResponse<UserResponse>.Success(response, "User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", userId);
                return ApiResponse<UserResponse>.Failure("An error occurred while updating the user");
            }
        }

        public async Task<ApiResponse<PaginatedResult<UserResponse>>> GetUsersAsync(int page, int pageSize)
        {
            try
            {
                var users = await _unitOfWork.Users.GetActiveUsersAsync();
                var totalCount = users.Count();

                var pagedUsers = users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(MapToUserResponse)
                    .ToList();

                // Use constructor with parameters instead of object initializer
                var result = new PaginatedResult<UserResponse>(
                    pagedUsers,
                    totalCount,
                    page,
                    pageSize
                );

                return ApiResponse<PaginatedResult<UserResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return ApiResponse<PaginatedResult<UserResponse>>.Failure("An error occurred while retrieving users");
            }
        }

        public async Task<ApiResponse<bool>> DeactivateUserAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(userId);
                
                if (user == null)
                {
                    return ApiResponse<bool>.Failure("User not found");
                }

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {UserId} deactivated", userId);
                return ApiResponse<bool>.Success(true, "User deactivated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user {UserId}", userId);
                return ApiResponse<bool>.Failure("An error occurred while deactivating the user");
            }
        }

        public async Task<ApiResponse<bool>> ActivateUserAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(userId);
                
                if (user == null)
                {
                    return ApiResponse<bool>.Failure("User not found");
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {UserId} activated", userId);
                return ApiResponse<bool>.Success(true, "User activated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user {UserId}", userId);
                return ApiResponse<bool>.Failure("An error occurred while activating the user");
            }
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> GetUsersByBusinessUnitAsync(int businessUnitId)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersByBusinessUnitAsync(businessUnitId);
                var responses = users.Select(MapToUserResponse);

                return ApiResponse<IEnumerable<UserResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users for business unit {BusinessUnitId}", businessUnitId);
                return ApiResponse<IEnumerable<UserResponse>>.Failure("An error occurred while retrieving users");
            }
        }

        private static UserResponse MapToUserResponse(Core.Entities.User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Department = user.Department,
                Designation = user.Designation,
                BusinessUnitId = user.BusinessUnitId,
                BusinessUnitName = user.BusinessUnit?.Name,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt, // Fixed: Return DateTime instead of string
                LastLoginDate = user.LastLoginDate // Fixed: Return DateTime? instead of string
            };
        }
    }
}
