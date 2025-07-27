using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models.Entities
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ReporterName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ReporterIdNumber { get; set; } = string.Empty;

        [Required]
        public ReportType Type { get; set; }

        [Required]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Location { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.New;

        // Foreign Keys
        public string CreatedById { get; set; } = string.Empty;
        public virtual ApplicationUser CreatedBy { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<Case> RelatedCases { get; set; } = new List<Case>();
        public virtual ICollection<Suspect> RelatedSuspects { get; set; } = new List<Suspect>();
        public virtual ICollection<Document> Attachments { get; set; } = new List<Document>();
    }

    public enum ReportType
    {
        Theft,              // سرقة
        Assault,            // اعتداء
        Fraud,              // احتيال
        DrugRelated,        // متعلق بالمخدرات
        TrafficViolation,   // مخالفة مرورية
        Other               // أخرى
    }

    public enum ReportStatus
    {
        New,                // جديد
        UnderInvestigation, // قيد التحقيق
        Closed,             // مغلق
        TransferredToProsecution // محول للنيابة
    }
}