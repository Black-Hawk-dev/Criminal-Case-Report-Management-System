using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class SuspectViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Nationality { get; set; }
        public Gender? Gender { get; set; }
        public SuspectStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CasesCount { get; set; }
        public int ReportsCount { get; set; }
        public int Age => DateOfBirth.HasValue ? DateTime.Now.Year - DateOfBirth.Value.Year : 0;
    }

    public class CreateSuspectViewModel
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن يكون أقل من 100 حرف")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        [Display(Name = "رقم الهوية")]
        public string IdNumber { get; set; } = string.Empty;

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "العنوان يجب أن يكون أقل من 200 حرف")]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "الجنسية يجب أن تكون أقل من 100 حرف")]
        [Display(Name = "الجنسية")]
        public string? Nationality { get; set; }

        [Display(Name = "الجنس")]
        public Gender? Gender { get; set; }

        [Display(Name = "الحالة")]
        public SuspectStatus Status { get; set; } = SuspectStatus.Active;

        [StringLength(1000, ErrorMessage = "الملاحظات يجب أن تكون أقل من 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
    }

    public class SuspectListViewModel
    {
        public List<SuspectViewModel> Suspects { get; set; } = new List<SuspectViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public SuspectStatus? StatusFilter { get; set; }
        public Gender? GenderFilter { get; set; }
        public string? NationalityFilter { get; set; }
    }

    public class SuspectDetailsViewModel
    {
        public SuspectViewModel Suspect { get; set; } = new SuspectViewModel();
        public List<CaseViewModel> Cases { get; set; } = new List<CaseViewModel>();
        public List<ReportViewModel> Reports { get; set; } = new List<ReportViewModel>();
    }
}