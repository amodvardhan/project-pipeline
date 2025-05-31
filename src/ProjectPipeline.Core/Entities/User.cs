using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.Entities;

/// <summary>
/// User entity extending IdentityUser for authentication
/// </summary>
public class User : IdentityUser
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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? ProfilePicture { get; set; }

    public DateTime? LastLoginDate { get; set; }

    // Navigation Properties
    public virtual BusinessUnit? BusinessUnit { get; set; }
    public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
    public virtual ICollection<Project> UpdatedProjects { get; set; } = new List<Project>();
}
