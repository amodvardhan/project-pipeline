using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Core.Interfaces.Services;

namespace ProjectPipeline.Infrastructure.Services;

/// <summary>
/// User service implementation
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<UserResponse>> GetUserByIdAsync(string id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.Failure("User not found");
            }

            var response = MapToUserResponse(user);
            return ApiResponse<UserResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponse>.Failure($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserResponse>> UpdateUserAsync(string id, UpdateUserRequest request)
    {
        try
        {
            var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.Failure("User not found");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Department = request.Department;
            user.Designation = request.Designation;
            user.BusinessUnitId = request.BusinessUnitId;
            user.PhoneNumber = request.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var response = MapToUserResponse(user);
            return ApiResponse<UserResponse>.Success(response, "User updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponse>.Failure($"Error updating user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PaginatedResult<UserResponse>>> GetUsersAsync(int page, int pageSize)
    {
        try
        {
            var users = await _unitOfWork.Users.GetActiveUsersAsync();
            var totalCount = users.Count();

            var paginatedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToUserResponse)
                .ToList();

            var result = new PaginatedResult<UserResponse>(paginatedUsers, totalCount, page, pageSize);
            return ApiResponse<PaginatedResult<UserResponse>>.Success(result);
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResult<UserResponse>>.Failure($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetUsersByBusinessUnitAsync(int businessUnitId)
    {
        try
        {
            var users = await _unitOfWork.Users.GetUsersByBusinessUnitAsync(businessUnitId);
            var response = users.Select(MapToUserResponse);
            return ApiResponse<IEnumerable<UserResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserResponse>>.Failure($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeactivateUserAsync(string id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.Failure("User not found");
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, "User deactivated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Failure($"Error deactivating user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> ActivateUserAsync(string id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetUserWithBusinessUnitAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.Failure("User not found");
            }

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, "User activated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Failure($"Error activating user: {ex.Message}");
        }
    }

    private static UserResponse MapToUserResponse(Core.Entities.User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Department = user.Department,
            Designation = user.Designation,
            BusinessUnitId = user.BusinessUnitId,
            BusinessUnitName = user.BusinessUnit?.Name,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginDate = user.LastLoginDate
        };
    }
}
