namespace ProjectPipeline.Core.DTOs.Response;

public class ResourceUtilizationResponse
{
    public string ResourceName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public decimal UtilizationPercentage { get; set; }
    public int ActiveProjects { get; set; }
    public decimal BillingRate { get; set; }
}
