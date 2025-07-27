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
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<CaseUpdate> CaseUpdates { get; set; }
        public DbSet<CaseSuspect> CaseSuspects { get; set; }
        public DbSet<ReportSuspect> ReportSuspects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure many-to-many relationships
            builder.Entity<CaseSuspect>()
                .HasOne(cs => cs.Case)
                .WithMany(c => c.CaseSuspects)
                .HasForeignKey(cs => cs.CaseId);

            builder.Entity<CaseSuspect>()
                .HasOne(cs => cs.Suspect)
                .WithMany(s => s.CaseSuspects)
                .HasForeignKey(cs => cs.SuspectId);

            builder.Entity<ReportSuspect>()
                .HasOne(rs => rs.Report)
                .WithMany(r => r.ReportSuspects)
                .HasForeignKey(rs => rs.ReportId);

            builder.Entity<ReportSuspect>()
                .HasOne(rs => rs.Suspect)
                .WithMany(s => s.ReportSuspects)
                .HasForeignKey(rs => rs.SuspectId);

            // Configure one-to-many relationships
            builder.Entity<Report>()
                .HasOne(r => r.CreatedBy)
                .WithMany(u => u.CreatedReports)
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Case>()
                .HasOne(c => c.Report)
                .WithMany(r => r.Cases)
                .HasForeignKey(c => c.ReportId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Case>()
                .HasOne(c => c.AssignedInvestigator)
                .WithMany(u => u.AssignedCases)
                .HasForeignKey(c => c.AssignedInvestigatorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Document>()
                .HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Document>()
                .HasOne(d => d.Report)
                .WithMany(r => r.Documents)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Document>()
                .HasOne(d => d.Case)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<CaseUpdate>()
                .HasOne(cu => cu.Case)
                .WithMany(c => c.Updates)
                .HasForeignKey(cu => cu.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CaseUpdate>()
                .HasOne(cu => cu.UpdatedBy)
                .WithMany()
                .HasForeignKey(cu => cu.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure unique constraints
            builder.Entity<Report>()
                .HasIndex(r => r.ReporterIdNumber)
                .IsUnique();

            builder.Entity<Case>()
                .HasIndex(c => c.CaseNumber)
                .IsUnique();

            builder.Entity<Suspect>()
                .HasIndex(s => s.IdNumber)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure default values
            builder.Entity<Report>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<Case>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<Suspect>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<Document>()
                .Property(d => d.UploadDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<CaseUpdate>()
                .Property(cu => cu.UpdateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<ApplicationUser>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
