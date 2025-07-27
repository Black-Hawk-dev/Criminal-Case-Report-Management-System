
namespace CriminalCaseManagement.Models.Entities
{
    public class CaseUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "نص التحديث")]
        public string UpdateText { get; set; } = string.Empty;

        [Display(Name = "تاريخ التحديث")]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        [Display(Name = "نوع التحديث")]
        public UpdateType Type { get; set; }

        [StringLength(1000)]
        [Display(Name = "ملاحظات إضافية")]
        public string? Notes { get; set; }

        // Foreign Keys
        [Required]
        [Display(Name = "القضية")]
        public int CaseId { get; set; }

        [Required]
        [Display(Name = "تم التحديث بواسطة")]
        public string UpdatedById { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; } = null!;

        [ForeignKey("UpdatedById")]
        public virtual ApplicationUser UpdatedBy { get; set; } = null!;
    }

    public enum UpdateType
    {
        StatusChange,        // تغيير الحالة
        Assignment,          // تعيين محقق
        Evidence,            // إضافة دليل
        Interview,           // مقابلة
        Arrest,              // اعتقال
        Release,             // إطلاق سراح
        Transfer,            // تحويل
        Other                // أخرى
    }
}
