using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        public long FileSize { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public DocumentType Type { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public string UploadedById { get; set; } = string.Empty;
        public virtual ApplicationUser UploadedBy { get; set; } = null!;

        // Foreign Keys - Optional relationships
        public int? ReportId { get; set; }
        public virtual Report? Report { get; set; }

        public int? CaseId { get; set; }
        public virtual Case? Case { get; set; }
    }

    public enum DocumentType
    {
        Image,          // صورة
        PDF,            // ملف PDF
        Document,       // مستند
        Video,          // فيديو
        Audio,          // صوت
        Other           // أخرى
    }
}