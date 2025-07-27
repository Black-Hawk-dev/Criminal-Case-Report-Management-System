using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models.Entities
{
    public class CaseUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string UpdateText { get; set; } = string.Empty;

        [Required]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public UpdateType Type { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }

        // Foreign Keys
        public int CaseId { get; set; }
        public virtual Case Case { get; set; } = null!;

        public string UpdatedById { get; set; } = string.Empty;
        public virtual ApplicationUser UpdatedBy { get; set; } = null!;
    }

    public enum UpdateType
    {
        StatusChange,        // تغيير الحالة
        Assignment,          // تعيين محقق
        EvidenceAdded,       // إضافة دليل
        SuspectAdded,        // إضافة متهم
        NoteAdded,           // إضافة ملاحظة
        DocumentUploaded,    // رفع مستند
        Other                // أخرى
    }
}