using System.ComponentModel.DataAnnotations;

namespace ResumeApp.server.Models
{
    public class Experience
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Resume Resume { get; set; } = null!;
    }
}
