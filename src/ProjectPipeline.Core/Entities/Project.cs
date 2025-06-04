using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Technology { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ProjectType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? EstimatedValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualValue { get; set; }

        public ProjectStatusEnum Status { get; set; } = ProjectStatusEnum.Pipeline;

        [MaxLength(500)]
        public string? StatusReason { get; set; }

        public DateTime? ExpectedClosureDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Foreign Keys
        public int BusinessUnitId { get; set; }

        // Profile counts
        public int ProfilesSubmitted { get; set; } = 0;
        public int ProfilesShortlisted { get; set; } = 0;
        public int ProfilesSelected { get; set; } = 0;

        // Navigation Properties
        public virtual BusinessUnit BusinessUnit { get; set; } = null!;
        public virtual ICollection<ProfileSubmission> ProfileSubmissions { get; set; } = new List<ProfileSubmission>();
    }
}
