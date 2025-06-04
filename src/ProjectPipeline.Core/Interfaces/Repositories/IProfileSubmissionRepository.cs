using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for profile submission operations
    /// </summary>
    public interface IProfileSubmissionRepository : IGenericRepository<ProfileSubmission>
    {
        Task<IEnumerable<ProfileSubmission>> GetProfilesByProjectIdAsync(int projectId);
        Task<ProfileSubmission?> GetProfileWithDetailsAsync(int profileId);
        Task<IEnumerable<ProfileSubmission>> GetProfilesByStatusAsync(ProfileStatusEnum status);
        Task<IEnumerable<ProfileSubmission>> GetProfilesBySubmitterAsync(string submitterId);
        Task<ProfileSubmission?> GetProfileByEmailAndProjectAsync(string email, int projectId);
        Task<int> GetProfileCountByStatusAsync(int projectId, ProfileStatusEnum status);
        Task<IEnumerable<ProfileSubmission>> GetOverdueProfilesAsync(int days);
        Task<decimal> GetAverageTimeToHireAsync(int projectId);
        Task<Dictionary<ProfileStatusEnum, int>> GetStatusDistributionAsync(int projectId);
    }
}
