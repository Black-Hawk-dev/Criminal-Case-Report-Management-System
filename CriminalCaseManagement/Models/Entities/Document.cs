
namespace CriminalCaseManagement.Models.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "اسم الملف")]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "نوع الملف")]
        public string FileType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "حجم الملف")]
        public long FileSize { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "مسار الملف")]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "وصف الملف")]
        public string? Description { get; set; }

        [Display(Name = "نوع المستند")]
        public DocumentType Type { get; set; }

        [Display(Name = "تاريخ الرفع")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        // Foreign Keys
        [Display(Name = "تم الرفع بواسطة")]
        public string UploadedById { get; set; } = string.Empty;

        [Display(Name = "البلاغ المرتبط")]
        public int? ReportId { get; set; }

        [Display(Name = "القضية المرتبطة")]
        public int? CaseId { get; set; }

        // Navigation Properties
        [ForeignKey("UploadedById")]
        public virtual ApplicationUser UploadedBy { get; set; } = null!;

        [ForeignKey("ReportId")]
        public virtual Report? Report { get; set; }

        [ForeignKey("CaseId")]
        public virtual Case? Case { get; set; }
    }

    public enum DocumentType
    {
        Image,      // صورة
        PDF,        // ملف PDF
        Document,   // مستند
        Video,      // فيديو
        Audio,      // صوت
        Other       // أخرى
    }
}
