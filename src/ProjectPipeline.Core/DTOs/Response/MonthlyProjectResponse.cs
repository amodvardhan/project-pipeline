namespace ProjectPipeline.Core.DTOs.Response;

public class MonthlyProjectResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int ProjectsWon { get; set; }
    public int ProjectsLost { get; set; }
    public decimal ValueWon { get; set; }
    public decimal ValueLost { get; set; }
}
