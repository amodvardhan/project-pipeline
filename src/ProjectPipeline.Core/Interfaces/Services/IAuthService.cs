using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;

namespace ProjectPipeline.Core.Interfaces.Services;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<bool> LogoutAsync(string userId);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email);
}
