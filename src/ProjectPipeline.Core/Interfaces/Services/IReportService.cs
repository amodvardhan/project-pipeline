using ProjectPipeline.Core.DTOs.Response;

namespace ProjectPipeline.Core.Interfaces.Services;

/// <summary>
/// Report service interface for analytics and reporting
/// </summary>
public interface IReportService
{
    Task<DashboardResponse> GetDashboardDataAsync(int? businessUnitId = null);
    Task<IEnumerable<ProjectStatusReportResponse>> GetProjectStatusReportAsync(int? businessUnitId = null);
    Task<IEnumerable<ResourceUtilizationResponse>> GetResourceUtilizationReportAsync(int? businessUnitId = null);
    Task<IEnumerable<MonthlyProjectResponse>> GetMonthlyProjectReportAsync(int year, int? businessUnitId = null);
}
