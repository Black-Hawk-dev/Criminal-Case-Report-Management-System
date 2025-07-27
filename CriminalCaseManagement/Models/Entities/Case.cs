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
        [Display(Name = "رقم القضية")]
        public string CaseNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "عنوان القضية")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        [Display(Name = "وصف القضية")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "تاريخ فتح القضية")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ الإغلاق")]
        [DataType(DataType.Date)]
        public DateTime? CloseDate { get; set; }

        [Display(Name = "الحالة")]
        public CaseStatus Status { get; set; } = CaseStatus.Open;

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ آخر تحديث")]
        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys
        [Display(Name = "البلاغ المرتبط")]
        public int? ReportId { get; set; }

        [Display(Name = "المحقق المسؤول")]
        public string? AssignedInvestigatorId { get; set; }

        // Navigation Properties
        [ForeignKey("ReportId")]
        public virtual Report? Report { get; set; }

        [ForeignKey("AssignedInvestigatorId")]
        public virtual ApplicationUser? AssignedInvestigator { get; set; }

        public virtual ICollection<Suspect> Suspects { get; set; } = new List<Suspect>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<CaseUpdate> Updates { get; set; } = new List<CaseUpdate>();
        public virtual ICollection<CaseSuspect> CaseSuspects { get; set; } = new List<CaseSuspect>();
    }

    public enum CaseStatus
    {
        Open,               // مفتوح
        UnderInvestigation, // قيد التحقيق
        Pending,            // في الانتظار
        Closed,             // مغلق
        TransferredToProsecution, // محول للنيابة
        Dismissed           // مرفوض
    }
}