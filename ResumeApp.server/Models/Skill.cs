using System.ComponentModel.DataAnnotations;

namespace ResumeApp.server.Models
{
    public class Skill
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Proficiency { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Resume Resume { get; set; } = null!;
    }
}
