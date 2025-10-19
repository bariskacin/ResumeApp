using System.ComponentModel.DataAnnotations;

namespace ResumeApp.server.Models
{
    public class Education
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Institution { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Degree { get; set; } = string.Empty;

        public string Field { get; set; } = string.Empty;

        [Required]
        public int StartYear { get; set; }

        public int? EndYear { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Resume Resume { get; set; } = null!;
    }
}
