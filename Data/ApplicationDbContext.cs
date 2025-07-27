namespace CriminalCaseManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Investigator> Investigators { get; set; }
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<CaseSuspect> CaseSuspects { get; set; }
        public DbSet<ReportSuspect> ReportSuspects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure CaseSuspect many-to-many relationship
            builder.Entity<CaseSuspect>()
                .HasOne(cs => cs.Case)
                .WithMany()
                .HasForeignKey(cs => cs.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CaseSuspect>()
                .HasOne(cs => cs.Suspect)
                .WithMany()
                .HasForeignKey(cs => cs.SuspectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ReportSuspect many-to-many relationship
            builder.Entity<ReportSuspect>()
                .HasOne(rs => rs.Report)
                .WithMany()
                .HasForeignKey(rs => rs.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReportSuspect>()
                .HasOne(rs => rs.Suspect)
                .WithMany()
                .HasForeignKey(rs => rs.SuspectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Case-Report relationship
            builder.Entity<Case>()
                .HasOne(c => c.Report)
                .WithMany(r => r.Cases)
                .HasForeignKey(c => c.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Case-Investigator relationship
            builder.Entity<Case>()
                .HasOne(c => c.Investigator)
                .WithMany(i => i.Cases)
                .HasForeignKey(c => c.InvestigatorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Investigator-User relationship
            builder.Entity<Investigator>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Document relationships
            builder.Entity<Document>()
                .HasOne(d => d.Case)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Document>()
                .HasOne(d => d.Report)
                .WithMany()
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for testing
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seed Investigators
            builder.Entity<Investigator>().HasData(
                new Investigator
                {
                    Id = 1,
                    Name = "أحمد محمد علي",
                    Rank = "نقيب",
                    Department = "قسم التحقيقات الجنائية",
                    BadgeNumber = "INV001",
                    Email = "ahmed.ali@police.gov",
                    Phone = "0501234567",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Investigator
                {
                    Id = 2,
                    Name = "فاطمة عبدالله",
                    Rank = "ملازم أول",
                    Department = "قسم التحقيقات الجنائية",
                    BadgeNumber = "INV002",
                    Email = "fatima.abdullah@police.gov",
                    Phone = "0502345678",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );

            // Seed Report Types
            var reportTypes = new[] { "سرقة", "اعتداء", "احتيال", "تهديد", "إتلاف ممتلكات", "مخدرات", "أخرى" };
        }
    }
}
