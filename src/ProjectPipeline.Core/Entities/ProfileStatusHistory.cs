using System.ComponentModel.DataAnnotations;
using ProjectPipeline.Core.Enums;

namespace ProjectPipeline.Core.Entities
{
    public class ProfileStatusHistory : BaseEntity
    {
        [Required]
        public int ProfileSubmissionId { get; set; }
        
        public ProfileStatusEnum FromStatus { get; set; }
        public ProfileStatusEnum ToStatus { get; set; }
        
        [MaxLength(1000)]
        public string? Comments { get; set; }
        
        [Required]
        [MaxLength(450)]
        public string ChangedBy { get; set; } = string.Empty;
        
        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;
        
        [MaxLength(200)]
        public string? Reason { get; set; }
        
        // Navigation Properties
        public virtual ProfileSubmission ProfileSubmission { get; set; } = null!;
        public virtual User ChangedByUser { get; set; } = null!;
    }
}
