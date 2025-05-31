using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.Entities;

/// <summary>
/// Base entity class that provides common properties for all entities
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }
}
