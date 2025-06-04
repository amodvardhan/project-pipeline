using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Entities
{
    public class ProfileSubmission : BaseEntity
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CandidateName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string CandidateEmail { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? CandidatePhone { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Technology { get; set; } = string.Empty;
        
        public int ExperienceYears { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExpectedSalary { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OfferedSalary { get; set; }
        
        public ProfileStatusEnum Status { get; set; } = ProfileStatusEnum.Submitted;
        
        [MaxLength(1000)]
        public string? StatusComments { get; set; }
        
        [MaxLength(200)]
        public string? RejectionReason { get; set; }
        
        [MaxLength(200)]
        public string? HoldReason { get; set; }
        
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public DateTime? ScreeningDate { get; set; }
        public DateTime? ShortlistDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime? SelectionDate { get; set; }
        public DateTime? RejectionDate { get; set; }
        public DateTime? ExpectedJoiningDate { get; set; }
        public DateTime? ActualJoiningDate { get; set; }
        
        [MaxLength(100)]
        public string? InterviewerName { get; set; }
        
        [Column(TypeName = "decimal(3,2)")]
        public decimal? InterviewScore { get; set; }
        
        [MaxLength(1000)]
        public string? InterviewFeedback { get; set; }
        
        [Column(TypeName = "decimal(3,2)")]
        public decimal? TechnicalScore { get; set; }
        
        [MaxLength(1000)]
        public string? TechnicalFeedback { get; set; }
        
        [MaxLength(500)]
        public string? ResumePath { get; set; }
        
        [MaxLength(500)]
        public string? OfferLetterPath { get; set; }
        
        [Required]
        [MaxLength(450)]
        public string SubmittedBy { get; set; } = string.Empty;
        
        [MaxLength(450)]
        public string? LastUpdatedBy { get; set; }
        
        public int DaysInPipeline => (DateTime.UtcNow - SubmissionDate).Days;
        
        // Navigation Properties
        public virtual Project Project { get; set; } = null!;
        public virtual User SubmittedByUser { get; set; } = null!;
        public virtual User? LastUpdatedByUser { get; set; }
        public virtual ICollection<ProfileStatusHistory> StatusHistory { get; set; } = new List<ProfileStatusHistory>();
    }
}
