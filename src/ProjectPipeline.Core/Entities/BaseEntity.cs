using System.ComponentModel.DataAnnotations;

namespace ProjectPipeline.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        [MaxLength(450)]
        public string? CreatedBy { get; set; } // Made nullable for existing data
        
        [MaxLength(450)]
        public string? UpdatedBy { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        
        [MaxLength(450)]
        public string? DeletedBy { get; set; }
        
        // Navigation Properties for Audit - all nullable
        public virtual User? CreatedByUser { get; set; }
        public virtual User? UpdatedByUser { get; set; }
        public virtual User? DeletedByUser { get; set; }
    }
}
