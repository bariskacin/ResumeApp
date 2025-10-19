using System.ComponentModel.DataAnnotations;

namespace ResumeApp.server.Models
{
    public class Resume
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        
        [Required]
        public string Headline { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Person Person { get; set; } = null!;
    }
}
