namespace ProjectPipeline.Core.DTOs.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public decimal? EstimatedValue { get; set; }
    public decimal? ActualValue { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? StatusReason { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ExpectedClosureDate { get; set; }
    public int BusinessUnitId { get; set; }
    public string? BusinessUnitName { get; set; }
    public string Technology { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public int ProfilesSubmitted { get; set; }
    public int ProfilesShortlisted { get; set; }
    public int ProfilesSelected { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? CreatedByName { get; set; }
}
