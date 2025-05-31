using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Entities;

/// <summary>
/// Project entity representing a business opportunity or project
/// </summary>
public class Project : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualValue { get; set; }

    public ProjectStatusEnum Status { get; set; } = ProjectStatusEnum.Pipeline;

    [MaxLength(2000)]
    public string? StatusReason { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ExpectedClosureDate { get; set; }

    public int BusinessUnitId { get; set; }

    [MaxLength(100)]
    public string Technology { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ProjectType { get; set; } = string.Empty;

    public int ProfilesSubmitted { get; set; } = 0;

    public int ProfilesShortlisted { get; set; } = 0;

    public int ProfilesSelected { get; set; } = 0;

    // Navigation Properties
    public virtual BusinessUnit BusinessUnit { get; set; } = null!;
    public virtual User CreatedByUser { get; set; } = null!;
    public virtual User? UpdatedByUser { get; set; }
    public virtual ICollection<ProfileSubmission> ProfileSubmissions { get; set; } = new List<ProfileSubmission>();
    public virtual ICollection<ResourceAllocation> ResourceAllocations { get; set; } = new List<ResourceAllocation>();
}
