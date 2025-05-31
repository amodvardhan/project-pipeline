using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request;

public class CreateProjectRequest
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

    public DateTime? StartDate { get; set; }

    public DateTime? ExpectedClosureDate { get; set; }

    [Required]
    public int BusinessUnitId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Technology { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ProjectType { get; set; } = string.Empty;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;
}
