using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.Entities;

/// <summary>
/// Business Unit entity for organizational structure
/// </summary>
public class BusinessUnit : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? HeadOfUnit { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
