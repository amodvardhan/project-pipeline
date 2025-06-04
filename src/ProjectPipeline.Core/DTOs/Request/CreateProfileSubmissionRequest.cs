using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request
{
    /// <summary>
    /// Request DTO for creating a new profile submission
    /// </summary>
    public class CreateProfileSubmissionRequest
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CandidateName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string CandidateEmail { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? CandidatePhone { get; set; }

        [Required]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Technology { get; set; } = string.Empty;

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ExpectedSalary { get; set; }

        [MaxLength(500)]
        public string? ResumePath { get; set; }

        [MaxLength(1000)]
        public string? InitialComments { get; set; }
    }
}
