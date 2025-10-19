using Microsoft.EntityFrameworkCore;
using ResumeApp.server.Models;

namespace ResumeApp.server
{
    public class ResumeDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ResumeDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Resume>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Person)
                      .WithMany()
                      .HasForeignKey(e => e.PersonId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Headline).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Summary).HasMaxLength(2000);
            });

            modelBuilder.Entity<Experience>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Resume)
                      .WithMany()
                      .HasForeignKey(e => e.ResumeId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Company).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Resume)
                      .WithMany()
                      .HasForeignKey(e => e.ResumeId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Institution).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Degree).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Field).HasMaxLength(200);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Resume)
                      .WithMany()
                      .HasForeignKey(e => e.ResumeId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });
        }
    }
}
