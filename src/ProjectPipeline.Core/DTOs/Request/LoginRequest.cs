using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request;

/// <summary>
/// Login request DTO
/// </summary>
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}
