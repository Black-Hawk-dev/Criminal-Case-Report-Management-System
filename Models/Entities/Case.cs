using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models.Entities
{
    public class Case
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CaseNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ClosedDate { get; set; }

        public CaseStatus Status { get; set; } = CaseStatus.Open;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Foreign Keys
        public int ReportId { get; set; }
        public virtual Report Report { get; set; } = null!;

        public string? AssignedInvestigatorId { get; set; }
        public virtual ApplicationUser? AssignedInvestigator { get; set; }

        // Navigation properties
        public virtual ICollection<Suspect> Suspects { get; set; } = new List<Suspect>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<CaseUpdate> Updates { get; set; } = new List<CaseUpdate>();
    }

    public enum CaseStatus
    {
        Open,                   // مفتوح
        UnderInvestigation,     // قيد التحقيق
        Closed,                 // مغلق
        TransferredToProsecution, // محول للنيابة
        PendingEvidence,        // في انتظار الأدلة
        AwaitingCourtDecision   // في انتظار قرار المحكمة
    }
}