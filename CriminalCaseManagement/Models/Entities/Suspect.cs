
namespace CriminalCaseManagement.Models.Entities
{
    public class Suspect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "رقم الهوية")]
        public string IdNumber { get; set; } = string.Empty;

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        [Display(Name = "رقم الهاتف")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [StringLength(100)]
        [Display(Name = "الجنسية")]
        public string? Nationality { get; set; }

        [Display(Name = "الجنس")]
        public Gender? Gender { get; set; }

        [Display(Name = "الحالة")]
        public SuspectStatus Status { get; set; } = SuspectStatus.Active;

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ آخر تحديث")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<CaseSuspect> CaseSuspects { get; set; } = new List<CaseSuspect>();
        public virtual ICollection<ReportSuspect> ReportSuspects { get; set; } = new List<ReportSuspect>();
    }

    public enum Gender
    {
        Male,   // ذكر
        Female  // أنثى
    }

    public enum SuspectStatus
    {
        Active,     // نشط
        Arrested,   // مقبوض عليه
        Released,   // مطلق سراحه
        Wanted,     // مطلوب
        Deceased    // متوفى
    }
}
