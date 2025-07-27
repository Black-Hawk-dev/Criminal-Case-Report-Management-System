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
        [Display(Name = "اسم المبلغ")]
        public string ReporterName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "رقم هوية المبلغ")]
        public string ReporterIdNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "نوع البلاغ")]
        public ReportType Type { get; set; }

        [Required]
        [Display(Name = "تاريخ البلاغ")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500)]
        [Display(Name = "وصف البلاغ")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "الموقع")]
        public string? Location { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Display(Name = "الحالة")]
        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ آخر تحديث")]
        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys
        [Display(Name = "تم الإنشاء بواسطة")]
        public string CreatedById { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; } = null!;
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<ReportSuspect> ReportSuspects { get; set; } = new List<ReportSuspect>();
    }

    public enum ReportType
    {
        Theft,              // سرقة
        Assault,            // اعتداء
        Fraud,              // احتيال
        DrugTrafficking,    // تهريب مخدرات
        CyberCrime,         // جرائم إلكترونية
        DomesticViolence,   // عنف أسري
        Other               // أخرى
    }

    public enum ReportStatus
    {
        Pending,            // في الانتظار
        UnderInvestigation, // قيد التحقيق
        Assigned,           // تم التعيين
        Closed,             // مغلق
        Transferred         // محول
    }
}