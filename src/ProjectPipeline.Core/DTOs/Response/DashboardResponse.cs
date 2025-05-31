namespace ProjectPipeline.Core.DTOs.Response;

/// <summary>
/// Dashboard data response DTO
/// </summary>
public class DashboardResponse
{
    public int TotalProjects { get; set; }
    public int PipelineProjects { get; set; }
    public int WonProjects { get; set; }
    public int LostProjects { get; set; }
    public int MissedProjects { get; set; }
    public decimal TotalValue { get; set; }
    public decimal WonValue { get; set; }
    public int ActiveResources { get; set; }
    public int TotalProfilesSubmitted { get; set; }
    public decimal WinRate { get; set; }
    public IEnumerable<MonthlyProjectResponse> MonthlyTrends { get; set; } = new List<MonthlyProjectResponse>();
}
