namespace ProjectPipeline.Core.DTOs.Response;

public class ProjectStatusReportResponse
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
    public decimal Percentage { get; set; }
}
