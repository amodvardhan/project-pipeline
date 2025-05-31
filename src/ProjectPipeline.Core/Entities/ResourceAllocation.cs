using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Entities;

/// <summary>
/// Entity representing resource allocation for projects (ramp up/down)
/// </summary>
public class ResourceAllocation : BaseEntity
{
    [Required]
    public int ProjectId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ResourceName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ResourceEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Technology { get; set; } = string.Empty;

    public ResourceTypeEnum ResourceType { get; set; } = ResourceTypeEnum.FullTime;

    [MaxLength(20)]
    public string AllocationType { get; set; } = "RampUp"; // RampUp, RampDown, Replacement

    public DateTime AllocationStartDate { get; set; }

    public DateTime? AllocationEndDate { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal AllocationPercentage { get; set; } = 100.00m; // 0-100%

    [Column(TypeName = "decimal(18,2)")]
    public decimal? BillingRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CostRate { get; set; }

    [MaxLength(500)]
    public string? Comments { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string? ReplacedBy { get; set; }

    [MaxLength(500)]
    public string? ReasonForChange { get; set; }

    // Navigation Properties
    public virtual Project Project { get; set; } = null!;
}
