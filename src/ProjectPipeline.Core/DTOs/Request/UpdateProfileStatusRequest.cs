using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.DTOs.Request
{
    /// <summary>
    /// Request DTO for updating profile status
    /// </summary>
    public class UpdateProfileStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Comments { get; set; }

        [MaxLength(200)]
        public string? Reason { get; set; }

        public DateTime? InterviewDate { get; set; }

        public DateTime? ExpectedJoiningDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? OfferedSalary { get; set; }

        [Range(0, 10)]
        public decimal? InterviewScore { get; set; }

        [Range(0, 10)]
        public decimal? TechnicalScore { get; set; }

        [MaxLength(1000)]
        public string? InterviewFeedback { get; set; }

        [MaxLength(1000)]
        public string? TechnicalFeedback { get; set; }

        [MaxLength(100)]
        public string? InterviewerName { get; set; }
    }
}
