using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request;

/// <summary>
/// Update user request DTO
/// </summary>
public class UpdateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Department { get; set; }

    [MaxLength(200)]
    public string? Designation { get; set; }

    public int? BusinessUnitId { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}
