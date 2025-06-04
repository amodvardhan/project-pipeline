using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Interfaces.Services
{
    /// <summary>
    /// Service interface for profile submission business logic
    /// </summary>
    public interface IProfileSubmissionService
    {
        Task<ApiResponse<ProfileSubmissionResponse>> CreateProfileSubmissionAsync(CreateProfileSubmissionRequest request, string userId);
        Task<ApiResponse<bool>> UpdateProfileStatusAsync(int profileId, UpdateProfileStatusRequest request, string userId);
        Task<ApiResponse<ProfileSubmissionDetailResponse>> GetProfileDetailsAsync(int profileId);
        Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetProfilesByProjectAsync(int projectId);
        Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetProfilesByStatusAsync(ProfileStatusEnum status);
        Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetMySubmissionsAsync(string userId);
        Task<ApiResponse<bool>> DeleteProfileSubmissionAsync(int profileId, string userId);
        Task<ApiResponse<ProfileAnalyticsResponse>> GetProfileAnalyticsAsync(int projectId);
        Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetOverdueProfilesAsync(int days);
        Task<ApiResponse<bool>> BulkUpdateStatusAsync(List<int> profileIds, string status, string reason, string userId);
    }

    /// <summary>
    /// Profile analytics response
    /// </summary>
    public class ProfileAnalyticsResponse
    {
        public int TotalProfiles { get; set; }
        public int SubmittedCount { get; set; }
        public int ShortlistedCount { get; set; }
        public int SelectedCount { get; set; }
        public int RejectedCount { get; set; }
        public int OnHoldCount { get; set; }
        public int JoinedCount { get; set; }
        public decimal AverageTimeToHire { get; set; }
        public decimal ConversionRate { get; set; }
        public Dictionary<string, int> StatusDistribution { get; set; } = new();
        public Dictionary<string, int> RejectionReasons { get; set; } = new();
    }
}