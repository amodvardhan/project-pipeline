using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request;

/// <summary>
/// User registration request DTO
/// </summary>
public class RegisterRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Department { get; set; }

    [MaxLength(200)]
    public string? Designation { get; set; }

    public int? BusinessUnitId { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;
}
