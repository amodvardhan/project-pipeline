using ProjectPipeline.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request;

public class UpdateProjectRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    public decimal? EstimatedValue { get; set; }

    public decimal? ActualValue { get; set; }

    public ProjectStatusEnum Status { get; set; }

    [MaxLength(2000)]
    public string? StatusReason { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ExpectedClosureDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Technology { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ProjectType { get; set; } = string.Empty;

    [Required]
    public string UpdatedBy { get; set; } = string.Empty;
}
