namespace ProjectPipeline.Core.DTOs.Response;

/// <summary>
/// Authentication response DTO
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserResponse User { get; set; } = null!;
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
