using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for profile submission operations
    /// </summary>
    public class ProfileSubmissionRepository : GenericRepository<ProfileSubmission>, IProfileSubmissionRepository
    {
        public ProfileSubmissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProfileSubmission>> GetProfilesByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Where(p => p.ProjectId == projectId && !p.IsDeleted)
                .Include(p => p.Project)
                .Include(p => p.SubmittedByUser)
                .Include(p => p.LastUpdatedByUser)
                .OrderByDescending(p => p.SubmissionDate)
                .ToListAsync();
        }

        public async Task<ProfileSubmission?> GetProfileWithDetailsAsync(int profileId)
        {
            return await _dbSet
                .Where(p => p.Id == profileId && !p.IsDeleted)
                .Include(p => p.Project)
                .Include(p => p.SubmittedByUser)
                .Include(p => p.LastUpdatedByUser)
                .Include(p => p.StatusHistory)
                    .ThenInclude(h => h.ChangedByUser)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProfileSubmission>> GetProfilesByStatusAsync(ProfileStatusEnum status)
        {
            return await _dbSet
                .Where(p => p.Status == status && !p.IsDeleted)
                .Include(p => p.Project)
                .Include(p => p.SubmittedByUser)
                .OrderByDescending(p => p.SubmissionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProfileSubmission>> GetProfilesBySubmitterAsync(string submitterId)
        {
            return await _dbSet
                .Where(p => p.SubmittedBy == submitterId && !p.IsDeleted)
                .Include(p => p.Project)
                .OrderByDescending(p => p.SubmissionDate)
                .ToListAsync();
        }

        public async Task<ProfileSubmission?> GetProfileByEmailAndProjectAsync(string email, int projectId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.CandidateEmail == email && 
                                         p.ProjectId == projectId && 
                                         !p.IsDeleted);
        }

        public async Task<int> GetProfileCountByStatusAsync(int projectId, ProfileStatusEnum status)
        {
            return await _dbSet
                .CountAsync(p => p.ProjectId == projectId && 
                                p.Status == status && 
                                !p.IsDeleted);
        }

        public async Task<IEnumerable<ProfileSubmission>> GetOverdueProfilesAsync(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _dbSet
                .Where(p => p.SubmissionDate <= cutoffDate && 
                           p.Status == ProfileStatusEnum.Submitted && 
                           !p.IsDeleted)
                .Include(p => p.Project)
                .Include(p => p.SubmittedByUser)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageTimeToHireAsync(int projectId)
        {
            var joinedProfiles = await _dbSet
                .Where(p => p.ProjectId == projectId && 
                           p.Status == ProfileStatusEnum.Joined && 
                           p.ActualJoiningDate.HasValue &&
                           !p.IsDeleted)
                .ToListAsync();

            if (!joinedProfiles.Any())
                return 0;

            var averageDays = joinedProfiles
                .Average(p => (p.ActualJoiningDate!.Value - p.SubmissionDate).TotalDays);

            return (decimal)averageDays;
        }

        public async Task<Dictionary<ProfileStatusEnum, int>> GetStatusDistributionAsync(int projectId)
        {
            var statusCounts = await _dbSet
                .Where(p => p.ProjectId == projectId && !p.IsDeleted)
                .GroupBy(p => p.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return statusCounts.ToDictionary(x => x.Status, x => x.Count);
        }
    }
}
