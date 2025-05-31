using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPipeline.Core.Entities;

/// <summary>
/// Entity representing profile submissions for projects
/// </summary>
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

    [MaxLength(50)]
    public string Status { get; set; } = "Submitted"; // Submitted, Shortlisted, Selected, Rejected

    [MaxLength(500)]
    public string? Comments { get; set; }

    public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

    public DateTime? InterviewDate { get; set; }

    public DateTime? JoiningDate { get; set; }

    [MaxLength(500)]
    public string? ResumePath { get; set; }

    // Navigation Properties
    public virtual Project Project { get; set; } = null!;
}
