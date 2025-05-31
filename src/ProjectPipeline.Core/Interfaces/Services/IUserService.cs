using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.DTOs.Common;

namespace ProjectPipeline.Core.Interfaces.Services;

/// <summary>
/// User service interface
/// </summary>
public interface IUserService
{
    Task<ApiResponse<UserResponse>> GetUserByIdAsync(string id);
    Task<ApiResponse<UserResponse>> UpdateUserAsync(string id, UpdateUserRequest request);
    Task<ApiResponse<PaginatedResult<UserResponse>>> GetUsersAsync(int page, int pageSize);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetUsersByBusinessUnitAsync(int businessUnitId);
    Task<ApiResponse<bool>> DeactivateUserAsync(string id);
    Task<ApiResponse<bool>> ActivateUserAsync(string id);
}
