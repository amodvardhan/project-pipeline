namespace ProjectPipeline.Core.DTOs.Response
{
    /// <summary>
    /// Response DTO for profile submission list
    /// </summary>
    public class ProfileSubmissionResponse
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public string? CandidatePhone { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Technology { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public decimal? OfferedSalary { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? StatusComments { get; set; }
        public string? RejectionReason { get; set; }
        public string? HoldReason { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime? ExpectedJoiningDate { get; set; }
        public DateTime? ActualJoiningDate { get; set; }
        public decimal? InterviewScore { get; set; }
        public decimal? TechnicalScore { get; set; }
        public int DaysInPipeline { get; set; }
        public string SubmittedBy { get; set; } = string.Empty;
        public int StatusHistoryCount { get; set; }
    }

    /// <summary>
    /// Detailed response DTO for single profile submission
    /// </summary>
    public class ProfileSubmissionDetailResponse : ProfileSubmissionResponse
    {
        public string? InterviewFeedback { get; set; }
        public string? TechnicalFeedback { get; set; }
        public string? InterviewerName { get; set; }
        public string? ResumePath { get; set; }
        public string? OfferLetterPath { get; set; }
        public List<StatusHistoryResponse> StatusHistory { get; set; } = new();
    }

    /// <summary>
    /// Status history response DTO
    /// </summary>
    public class StatusHistoryResponse
    {
        public string FromStatus { get; set; } = string.Empty;
        public string ToStatus { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public string? Reason { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime ChangedDate { get; set; }
    }
}
