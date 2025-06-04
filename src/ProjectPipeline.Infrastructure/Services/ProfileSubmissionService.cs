using Microsoft.Extensions.Logging;
using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Core.Interfaces.Services;

namespace ProjectPipeline.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for profile submission business logic
    /// </summary>
    public class ProfileSubmissionService : IProfileSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProfileSubmissionService> _logger;

        public ProfileSubmissionService(IUnitOfWork unitOfWork, ILogger<ProfileSubmissionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<ProfileSubmissionResponse>> CreateProfileSubmissionAsync(CreateProfileSubmissionRequest request, string userId)
        {
            try
            {
                // Check if project exists - ProjectId is int
                var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
                if (project == null)
                {
                    return ApiResponse<ProfileSubmissionResponse>.Failure("Project not found");
                }

                // Check for duplicate submission using LINQ instead of repository method
                var existingProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var existingProfile = existingProfiles.FirstOrDefault(p => 
                    p.CandidateEmail == request.CandidateEmail && 
                    p.ProjectId == request.ProjectId && 
                    !p.IsDeleted);

                if (existingProfile != null)
                {
                    return ApiResponse<ProfileSubmissionResponse>.Failure("Profile already submitted for this project");
                }

                var profile = new ProfileSubmission
                {
                    ProjectId = request.ProjectId, // int
                    CandidateName = request.CandidateName,
                    CandidateEmail = request.CandidateEmail,
                    CandidatePhone = request.CandidatePhone,
                    Position = request.Position,
                    Technology = request.Technology,
                    ExperienceYears = request.ExperienceYears,
                    ExpectedSalary = request.ExpectedSalary,
                    ResumePath = request.ResumePath,
                    StatusComments = request.InitialComments,
                    Status = ProfileStatusEnum.Submitted,
                    SubmittedBy = userId, // string
                    CreatedBy = userId, // string
                    SubmissionDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<ProfileSubmission>().AddAsync(profile);
                await _unitOfWork.SaveChangesAsync();

                // Create initial status history
                var statusHistory = new ProfileStatusHistory
                {
                    ProfileSubmissionId = profile.Id, // int
                    FromStatus = ProfileStatusEnum.Submitted,
                    ToStatus = ProfileStatusEnum.Submitted,
                    Comments = request.InitialComments ?? "Profile submitted to the system",
                    ChangedBy = userId, // string
                    CreatedBy = userId // string
                };

                await _unitOfWork.Repository<ProfileStatusHistory>().AddAsync(statusHistory);
                await _unitOfWork.SaveChangesAsync();

                // Update project profile counts
                await UpdateProjectProfileCountsAsync(request.ProjectId); // int

                var response = await MapToProfileResponseAsync(profile);
                _logger.LogInformation("Profile submission created for project {ProjectId} by user {UserId}", request.ProjectId, userId);

                return ApiResponse<ProfileSubmissionResponse>.Success(response, "Profile submitted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating profile submission for project {ProjectId}", request.ProjectId);
                return ApiResponse<ProfileSubmissionResponse>.Failure("An error occurred while creating the profile submission");
            }
        }

        public async Task<ApiResponse<bool>> UpdateProfileStatusAsync(int profileId, UpdateProfileStatusRequest request, string userId)
        {
            try
            {
                var profile = await _unitOfWork.Repository<ProfileSubmission>().GetByIdAsync(profileId); // int
                if (profile == null)
                {
                    return ApiResponse<bool>.Failure("Profile not found");
                }

                if (!Enum.TryParse<ProfileStatusEnum>(request.Status, out var newStatus))
                {
                    return ApiResponse<bool>.Failure("Invalid status");
                }

                var oldStatus = profile.Status;

                // Validate status transition
                if (!IsValidStatusTransition(oldStatus, newStatus))
                {
                    return ApiResponse<bool>.Failure($"Invalid status transition from {oldStatus} to {newStatus}");
                }

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    // Update profile
                    profile.Status = newStatus;
                    profile.StatusComments = request.Comments;
                    profile.LastUpdatedBy = userId; // string
                    profile.UpdatedAt = DateTime.UtcNow;
                    profile.UpdatedBy = userId; // string

                    // Update specific fields based on status
                    UpdateProfileFieldsByStatus(profile, newStatus, request);

                    _unitOfWork.Repository<ProfileSubmission>().Update(profile);

                    // Create status history
                    var statusHistory = new ProfileStatusHistory
                    {
                        ProfileSubmissionId = profileId, // int
                        FromStatus = oldStatus,
                        ToStatus = newStatus,
                        Comments = request.Comments,
                        Reason = request.Reason,
                        ChangedBy = userId, // string
                        CreatedBy = userId // string
                    };

                    await _unitOfWork.Repository<ProfileStatusHistory>().AddAsync(statusHistory);
                    await _unitOfWork.SaveChangesAsync();

                    // Update project profile counts
                    await UpdateProjectProfileCountsAsync(profile.ProjectId); // int

                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation("Profile {ProfileId} status updated from {OldStatus} to {NewStatus} by user {UserId}", 
                        profileId, oldStatus, newStatus, userId);

                    return ApiResponse<bool>.Success(true, "Profile status updated successfully");
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile status for profile {ProfileId}", profileId);
                return ApiResponse<bool>.Failure("An error occurred while updating profile status");
            }
        }

        public async Task<ApiResponse<ProfileSubmissionDetailResponse>> GetProfileDetailsAsync(int profileId)
        {
            try
            {
                var profiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profile = profiles.FirstOrDefault(p => p.Id == profileId && !p.IsDeleted);

                if (profile == null)
                {
                    return ApiResponse<ProfileSubmissionDetailResponse>.Failure("Profile not found");
                }

                var response = await MapToDetailResponseAsync(profile);
                return ApiResponse<ProfileSubmissionDetailResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile details for profile {ProfileId}", profileId);
                return ApiResponse<ProfileSubmissionDetailResponse>.Failure("An error occurred while retrieving profile details");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetProfilesByProjectAsync(int projectId)
        {
            try
            {
                var allProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profiles = allProfiles.Where(p => p.ProjectId == projectId && !p.IsDeleted).ToList();

                var responses = new List<ProfileSubmissionResponse>();
                foreach (var profile in profiles)
                {
                    responses.Add(await MapToProfileResponseAsync(profile));
                }

                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profiles for project {ProjectId}", projectId);
                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Failure("An error occurred while retrieving profiles");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetProfilesByStatusAsync(ProfileStatusEnum status)
        {
            try
            {
                var allProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profiles = allProfiles.Where(p => p.Status == status && !p.IsDeleted).ToList();

                var responses = new List<ProfileSubmissionResponse>();
                foreach (var profile in profiles)
                {
                    responses.Add(await MapToProfileResponseAsync(profile));
                }

                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profiles by status {Status}", status);
                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Failure("An error occurred while retrieving profiles");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetMySubmissionsAsync(string userId)
        {
            try
            {
                var allProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profiles = allProfiles.Where(p => p.SubmittedBy == userId && !p.IsDeleted).ToList();

                var responses = new List<ProfileSubmissionResponse>();
                foreach (var profile in profiles)
                {
                    responses.Add(await MapToProfileResponseAsync(profile));
                }

                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving submissions for user {UserId}", userId);
                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Failure("An error occurred while retrieving your submissions");
            }
        }

        public async Task<ApiResponse<bool>> DeleteProfileSubmissionAsync(int profileId, string userId)
        {
            try
            {
                var profile = await _unitOfWork.Repository<ProfileSubmission>().GetByIdAsync(profileId); // int
                if (profile == null)
                {
                    return ApiResponse<bool>.Failure("Profile not found");
                }

                profile.IsDeleted = true;
                profile.DeletedAt = DateTime.UtcNow;
                profile.DeletedBy = userId; // string

                _unitOfWork.Repository<ProfileSubmission>().Update(profile);
                await _unitOfWork.SaveChangesAsync();

                // Update project profile counts
                await UpdateProjectProfileCountsAsync(profile.ProjectId); // int

                _logger.LogInformation("Profile {ProfileId} deleted by user {UserId}", profileId, userId);
                return ApiResponse<bool>.Success(true, "Profile submission deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profile {ProfileId}", profileId);
                return ApiResponse<bool>.Failure("An error occurred while deleting the profile submission");
            }
        }

        public async Task<ApiResponse<ProfileAnalyticsResponse>> GetProfileAnalyticsAsync(int projectId)
        {
            try
            {
                var allProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profiles = allProfiles.Where(p => p.ProjectId == projectId && !p.IsDeleted).ToList();

                var statusDistribution = profiles
                    .GroupBy(p => p.Status)
                    .ToDictionary(g => g.Key, g => g.Count());

                var totalProfiles = profiles.Count;
                var selectedCount = profiles.Count(p => p.Status == ProfileStatusEnum.Selected || p.Status == ProfileStatusEnum.Joined);
                var conversionRate = totalProfiles > 0 ? (decimal)selectedCount / totalProfiles * 100 : 0;

                // Calculate average time to hire
                var joinedProfiles = profiles.Where(p => p.Status == ProfileStatusEnum.Joined && p.ActualJoiningDate.HasValue).ToList();
                var averageTimeToHire = joinedProfiles.Any() ? 
                    (decimal)joinedProfiles.Average(p => (p.ActualJoiningDate!.Value - p.SubmissionDate).TotalDays) : 0;

                var response = new ProfileAnalyticsResponse
                {
                    TotalProfiles = totalProfiles,
                    SubmittedCount = statusDistribution.GetValueOrDefault(ProfileStatusEnum.Submitted, 0),
                    ShortlistedCount = statusDistribution.GetValueOrDefault(ProfileStatusEnum.Shortlisted, 0),
                    SelectedCount = selectedCount,
                    RejectedCount = statusDistribution.GetValueOrDefault(ProfileStatusEnum.Rejected, 0),
                    OnHoldCount = statusDistribution.GetValueOrDefault(ProfileStatusEnum.OnHold, 0),
                    JoinedCount = statusDistribution.GetValueOrDefault(ProfileStatusEnum.Joined, 0),
                    AverageTimeToHire = averageTimeToHire,
                    ConversionRate = conversionRate,
                    StatusDistribution = statusDistribution.ToDictionary(x => x.Key.ToString(), x => x.Value)
                };

                return ApiResponse<ProfileAnalyticsResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile analytics for project {ProjectId}", projectId);
                return ApiResponse<ProfileAnalyticsResponse>.Failure("An error occurred while retrieving profile analytics");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProfileSubmissionResponse>>> GetOverdueProfilesAsync(int days)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-days);
                var allProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profiles = allProfiles.Where(p => 
                    p.SubmissionDate <= cutoffDate && 
                    p.Status == ProfileStatusEnum.Submitted && 
                    !p.IsDeleted).ToList();

                var responses = new List<ProfileSubmissionResponse>();
                foreach (var profile in profiles)
                {
                    responses.Add(await MapToProfileResponseAsync(profile));
                }

                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving overdue profiles");
                return ApiResponse<IEnumerable<ProfileSubmissionResponse>>.Failure("An error occurred while retrieving overdue profiles");
            }
        }

        public async Task<ApiResponse<bool>> BulkUpdateStatusAsync(List<int> profileIds, string status, string reason, string userId)
        {
            try
            {
                if (!Enum.TryParse<ProfileStatusEnum>(status, out var newStatus))
                {
                    return ApiResponse<bool>.Failure("Invalid status");
                }

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    foreach (var profileId in profileIds)
                    {
                        var profile = await _unitOfWork.Repository<ProfileSubmission>().GetByIdAsync(profileId); // int
                        if (profile != null)
                        {
                            var oldStatus = profile.Status;
                            profile.Status = newStatus;
                            profile.StatusComments = reason;
                            profile.LastUpdatedBy = userId; // string
                            profile.UpdatedAt = DateTime.UtcNow;
                            profile.UpdatedBy = userId; // string

                            if (newStatus == ProfileStatusEnum.Rejected)
                            {
                                profile.RejectionReason = reason;
                                profile.RejectionDate = DateTime.UtcNow;
                            }

                            _unitOfWork.Repository<ProfileSubmission>().Update(profile);

                            // Create status history
                            var statusHistory = new ProfileStatusHistory
                            {
                                ProfileSubmissionId = profileId, // int
                                FromStatus = oldStatus,
                                ToStatus = newStatus,
                                Comments = reason,
                                Reason = reason,
                                ChangedBy = userId, // string
                                CreatedBy = userId // string
                            };

                            await _unitOfWork.Repository<ProfileStatusHistory>().AddAsync(statusHistory);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation("Bulk status update completed for {Count} profiles by user {UserId}", profileIds.Count, userId);
                    return ApiResponse<bool>.Success(true, $"Successfully updated {profileIds.Count} profiles");
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk status update");
                return ApiResponse<bool>.Failure("An error occurred while updating profiles");
            }
        }

        #region Private Methods

        private static bool IsValidStatusTransition(ProfileStatusEnum from, ProfileStatusEnum to)
        {
            // Define valid status transitions
            var validTransitions = new Dictionary<ProfileStatusEnum, List<ProfileStatusEnum>>
            {
                [ProfileStatusEnum.Submitted] = new() { ProfileStatusEnum.UnderScreening, ProfileStatusEnum.Rejected, ProfileStatusEnum.OnHold },
                [ProfileStatusEnum.UnderScreening] = new() { ProfileStatusEnum.Shortlisted, ProfileStatusEnum.Rejected, ProfileStatusEnum.OnHold },
                [ProfileStatusEnum.Shortlisted] = new() { ProfileStatusEnum.InterviewScheduled, ProfileStatusEnum.Rejected, ProfileStatusEnum.OnHold },
                [ProfileStatusEnum.InterviewScheduled] = new() { ProfileStatusEnum.InterviewCompleted, ProfileStatusEnum.Rejected, ProfileStatusEnum.OnHold },
                [ProfileStatusEnum.InterviewCompleted] = new() { ProfileStatusEnum.TechnicalRoundScheduled, ProfileStatusEnum.Selected, ProfileStatusEnum.Rejected },
                [ProfileStatusEnum.TechnicalRoundScheduled] = new() { ProfileStatusEnum.TechnicalRoundCompleted, ProfileStatusEnum.Rejected, ProfileStatusEnum.OnHold },
                [ProfileStatusEnum.TechnicalRoundCompleted] = new() { ProfileStatusEnum.Selected, ProfileStatusEnum.Rejected },
                [ProfileStatusEnum.Selected] = new() { ProfileStatusEnum.OfferExtended, ProfileStatusEnum.Rejected },
                [ProfileStatusEnum.OfferExtended] = new() { ProfileStatusEnum.OfferAccepted, ProfileStatusEnum.OfferDeclined },
                [ProfileStatusEnum.OfferAccepted] = new() { ProfileStatusEnum.Joined, ProfileStatusEnum.DroppedOut },
                [ProfileStatusEnum.OnHold] = new() { ProfileStatusEnum.UnderScreening, ProfileStatusEnum.Shortlisted, ProfileStatusEnum.Rejected }
            };

            return validTransitions.ContainsKey(from) && validTransitions[from].Contains(to);
        }

        private static void UpdateProfileFieldsByStatus(ProfileSubmission profile, ProfileStatusEnum status, UpdateProfileStatusRequest request)
        {
            switch (status)
            {
                case ProfileStatusEnum.Rejected:
                    profile.RejectionReason = request.Reason;
                    profile.RejectionDate = DateTime.UtcNow;
                    break;
                case ProfileStatusEnum.OnHold:
                    profile.HoldReason = request.Reason;
                    break;
                case ProfileStatusEnum.Shortlisted:
                    profile.ShortlistDate = DateTime.UtcNow;
                    break;
                case ProfileStatusEnum.InterviewScheduled:
                    profile.InterviewDate = request.InterviewDate;
                    profile.InterviewerName = request.InterviewerName;
                    break;
                case ProfileStatusEnum.InterviewCompleted:
                    profile.InterviewScore = request.InterviewScore;
                    profile.InterviewFeedback = request.InterviewFeedback;
                    break;
                case ProfileStatusEnum.TechnicalRoundCompleted:
                    profile.TechnicalScore = request.TechnicalScore;
                    profile.TechnicalFeedback = request.TechnicalFeedback;
                    break;
                case ProfileStatusEnum.Selected:
                    profile.SelectionDate = DateTime.UtcNow;
                    profile.ExpectedJoiningDate = request.ExpectedJoiningDate;
                    profile.OfferedSalary = request.OfferedSalary;
                    break;
                case ProfileStatusEnum.Joined:
                    profile.ActualJoiningDate = DateTime.UtcNow;
                    break;
            }
        }

        private async Task<ProfileSubmissionResponse> MapToProfileResponseAsync(ProfileSubmission profile)
        {
            var submittedByUser = await _unitOfWork.Users.GetByIdAsync(profile.SubmittedBy); // string
            var project = await _unitOfWork.Projects.GetByIdAsync(profile.ProjectId); // int

            return new ProfileSubmissionResponse
            {
                Id = profile.Id,
                ProjectId = profile.ProjectId,
                ProjectName = project?.Name ?? "Unknown",
                CandidateName = profile.CandidateName,
                CandidateEmail = profile.CandidateEmail,
                CandidatePhone = profile.CandidatePhone,
                Position = profile.Position,
                Technology = profile.Technology,
                ExperienceYears = profile.ExperienceYears,
                ExpectedSalary = profile.ExpectedSalary,
                OfferedSalary = profile.OfferedSalary,
                Status = profile.Status.ToString(),
                StatusComments = profile.StatusComments,
                RejectionReason = profile.RejectionReason,
                HoldReason = profile.HoldReason,
                SubmissionDate = profile.SubmissionDate,
                InterviewDate = profile.InterviewDate,
                ExpectedJoiningDate = profile.ExpectedJoiningDate,
                ActualJoiningDate = profile.ActualJoiningDate,
                InterviewScore = profile.InterviewScore,
                TechnicalScore = profile.TechnicalScore,
                DaysInPipeline = profile.DaysInPipeline,
                SubmittedBy = submittedByUser != null ? $"{submittedByUser.FirstName} {submittedByUser.LastName}" : "Unknown",
                StatusHistoryCount = 0 // Will be populated if needed
            };
        }

        private async Task<ProfileSubmissionDetailResponse> MapToDetailResponseAsync(ProfileSubmission profile)
        {
            var baseResponse = await MapToProfileResponseAsync(profile);
            
            return new ProfileSubmissionDetailResponse
            {
                Id = baseResponse.Id,
                ProjectId = baseResponse.ProjectId,
                ProjectName = baseResponse.ProjectName,
                CandidateName = baseResponse.CandidateName,
                CandidateEmail = baseResponse.CandidateEmail,
                CandidatePhone = baseResponse.CandidatePhone,
                Position = baseResponse.Position,
                Technology = baseResponse.Technology,
                ExperienceYears = baseResponse.ExperienceYears,
                ExpectedSalary = baseResponse.ExpectedSalary,
                OfferedSalary = baseResponse.OfferedSalary,
                Status = baseResponse.Status,
                StatusComments = baseResponse.StatusComments,
                RejectionReason = baseResponse.RejectionReason,
                HoldReason = baseResponse.HoldReason,
                SubmissionDate = baseResponse.SubmissionDate,
                InterviewDate = baseResponse.InterviewDate,
                ExpectedJoiningDate = baseResponse.ExpectedJoiningDate,
                ActualJoiningDate = baseResponse.ActualJoiningDate,
                InterviewScore = baseResponse.InterviewScore,
                TechnicalScore = baseResponse.TechnicalScore,
                DaysInPipeline = baseResponse.DaysInPipeline,
                SubmittedBy = baseResponse.SubmittedBy,
                StatusHistoryCount = baseResponse.StatusHistoryCount,
                InterviewFeedback = profile.InterviewFeedback,
                TechnicalFeedback = profile.TechnicalFeedback,
                InterviewerName = profile.InterviewerName,
                ResumePath = profile.ResumePath,
                OfferLetterPath = profile.OfferLetterPath,
                StatusHistory = new List<StatusHistoryResponse>() // Will be populated if needed
            };
        }

        private async Task UpdateProjectProfileCountsAsync(int projectId)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId); // int
            if (project != null)
            {
                var allProfiles = await _unitOfWork.Repository<ProfileSubmission>().GetAllAsync();
                var profiles = allProfiles.Where(p => p.ProjectId == projectId && !p.IsDeleted).ToList();

                project.ProfilesSubmitted = profiles.Count;
                project.ProfilesShortlisted = profiles.Count(p => 
                    p.Status == ProfileStatusEnum.Shortlisted || 
                    p.Status == ProfileStatusEnum.InterviewScheduled ||
                    p.Status == ProfileStatusEnum.InterviewCompleted ||
                    p.Status == ProfileStatusEnum.TechnicalRoundScheduled ||
                    p.Status == ProfileStatusEnum.TechnicalRoundCompleted ||
                    p.Status == ProfileStatusEnum.Selected ||
                    p.Status == ProfileStatusEnum.OfferExtended ||
                    p.Status == ProfileStatusEnum.OfferAccepted ||
                    p.Status == ProfileStatusEnum.Joined);

                project.ProfilesSelected = profiles.Count(p => 
                    p.Status == ProfileStatusEnum.Selected ||
                    p.Status == ProfileStatusEnum.OfferExtended ||
                    p.Status == ProfileStatusEnum.OfferAccepted ||
                    p.Status == ProfileStatusEnum.Joined);

                _unitOfWork.Projects.Update(project);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion
    }
}
