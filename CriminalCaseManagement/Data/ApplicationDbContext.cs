using Microsoft.EntityFrameworkCore;
using CriminalCaseManagement.Models;

namespace CriminalCaseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<SuspectReport> SuspectReports { get; set; }
        public DbSet<SuspectCase> SuspectCases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Role).HasConversion<int>();
            });

            // Report configurations
            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.Type).HasConversion<int>();
                entity.HasOne(r => r.CreatedBy)
                      .WithMany(u => u.CreatedReports)
                      .HasForeignKey(r => r.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Case configurations
            modelBuilder.Entity<Case>(entity =>
            {
                entity.HasIndex(e => e.CaseNumber).IsUnique();
                entity.Property(e => e.Status).HasConversion<int>();
                
                entity.HasOne(c => c.Report)
                      .WithMany(r => r.Cases)
                      .HasForeignKey(c => c.ReportId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(c => c.AssignedInvestigator)
                      .WithMany(u => u.AssignedCases)
                      .HasForeignKey(c => c.AssignedInvestigatorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Suspect configurations
            modelBuilder.Entity<Suspect>(entity =>
            {
                entity.HasIndex(e => e.IdNumber).IsUnique();
            });

            // SuspectReport configurations
            modelBuilder.Entity<SuspectReport>(entity =>
            {
                entity.HasOne(sr => sr.Suspect)
                      .WithMany(s => s.SuspectReports)
                      .HasForeignKey(sr => sr.SuspectId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(sr => sr.Report)
                      .WithMany(r => r.SuspectReports)
                      .HasForeignKey(sr => sr.ReportId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => new { e.SuspectId, e.ReportId }).IsUnique();
            });

            // SuspectCase configurations
            modelBuilder.Entity<SuspectCase>(entity =>
            {
                entity.HasOne(sc => sc.Suspect)
                      .WithMany(s => s.SuspectCases)
                      .HasForeignKey(sc => sc.SuspectId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(sc => sc.Case)
                      .WithMany(c => c.SuspectCases)
                      .HasForeignKey(sc => sc.CaseId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => new { e.SuspectId, e.CaseId }).IsUnique();
            });

            // Attachment configurations
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasOne(a => a.Report)
                      .WithMany(r => r.Attachments)
                      .HasForeignKey(a => a.ReportId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(a => a.Case)
                      .WithMany(c => c.Attachments)
                      .HasForeignKey(a => a.CaseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "مدير النظام",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.SystemAdmin,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}