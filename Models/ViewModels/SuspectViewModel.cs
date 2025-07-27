using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class SuspectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [Display(Name = "الاسم الكامل")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [Display(Name = "رقم الهوية")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        public string IdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "رقم الهاتف")]
        [StringLength(20, ErrorMessage = "رقم الهاتف يجب أن يكون أقل من 20 حرف")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "العنوان")]
        [StringLength(200, ErrorMessage = "العنوان يجب أن يكون أقل من 200 حرف")]
        public string? Address { get; set; }

        [Display(Name = "الجنسية")]
        [StringLength(100, ErrorMessage = "الجنسية يجب أن تكون أقل من 100 حرف")]
        public string? Nationality { get; set; }

        [Display(Name = "الحالة")]
        public SuspectStatus Status { get; set; } = SuspectStatus.Active;

        [Display(Name = "ملاحظات")]
        [StringLength(500, ErrorMessage = "الملاحظات يجب أن تكون أقل من 500 حرف")]
        public string? Notes { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }

        // Related data
        public int CasesCount { get; set; }
        public int ReportsCount { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }

    public class CreateSuspectViewModel
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [Display(Name = "الاسم الكامل")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [Display(Name = "رقم الهوية")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        public string IdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "رقم الهاتف")]
        [StringLength(20, ErrorMessage = "رقم الهاتف يجب أن يكون أقل من 20 حرف")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "العنوان")]
        [StringLength(200, ErrorMessage = "العنوان يجب أن يكون أقل من 200 حرف")]
        public string? Address { get; set; }

        [Display(Name = "الجنسية")]
        [StringLength(100, ErrorMessage = "الجنسية يجب أن تكون أقل من 100 حرف")]
        public string? Nationality { get; set; }

        [Display(Name = "ملاحظات")]
        [StringLength(500, ErrorMessage = "الملاحظات يجب أن تكون أقل من 500 حرف")]
        public string? Notes { get; set; }

        // Related cases and reports
        public List<int> RelatedCaseIds { get; set; } = new List<int>();
        public List<int> RelatedReportIds { get; set; } = new List<int>();
    }

    public class SuspectListViewModel
    {
        public List<SuspectViewModel> Suspects { get; set; } = new List<SuspectViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public SuspectStatus? FilterStatus { get; set; }
        public string? FilterNationality { get; set; }
        public int? FilterAgeFrom { get; set; }
        public int? FilterAgeTo { get; set; }
    }

    public class SuspectDetailsViewModel
    {
        public SuspectViewModel Suspect { get; set; } = new SuspectViewModel();
        public List<CaseViewModel> RelatedCases { get; set; } = new List<CaseViewModel>();
        public List<ReportViewModel> RelatedReports { get; set; } = new List<ReportViewModel>();
    }
}