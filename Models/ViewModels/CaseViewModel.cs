using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class CaseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "رقم القضية مطلوب")]
        [Display(Name = "رقم القضية")]
        [StringLength(50, ErrorMessage = "رقم القضية يجب أن يكون أقل من 50 حرف")]
        public string CaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "عنوان القضية مطلوب")]
        [Display(Name = "عنوان القضية")]
        [StringLength(200, ErrorMessage = "عنوان القضية يجب أن يكون أقل من 200 حرف")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف القضية مطلوب")]
        [Display(Name = "وصف القضية")]
        [StringLength(1000, ErrorMessage = "وصف القضية يجب أن يكون أقل من 1000 حرف")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "تاريخ الإغلاق")]
        public DateTime? ClosedDate { get; set; }

        [Display(Name = "الحالة")]
        public CaseStatus Status { get; set; }

        [Display(Name = "ملاحظات")]
        [StringLength(500, ErrorMessage = "الملاحظات يجب أن تكون أقل من 500 حرف")]
        public string? Notes { get; set; }

        // Related data
        public int ReportId { get; set; }
        public string ReportTitle { get; set; } = string.Empty;
        public string? AssignedInvestigatorId { get; set; }
        public string? AssignedInvestigatorName { get; set; }
        public int SuspectsCount { get; set; }
        public int DocumentsCount { get; set; }
        public int UpdatesCount { get; set; }
    }

    public class CreateCaseViewModel
    {
        [Required(ErrorMessage = "عنوان القضية مطلوب")]
        [Display(Name = "عنوان القضية")]
        [StringLength(200, ErrorMessage = "عنوان القضية يجب أن يكون أقل من 200 حرف")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف القضية مطلوب")]
        [Display(Name = "وصف القضية")]
        [StringLength(1000, ErrorMessage = "وصف القضية يجب أن يكون أقل من 1000 حرف")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "البلاغ المرتبط مطلوب")]
        [Display(Name = "البلاغ المرتبط")]
        public int ReportId { get; set; }

        [Display(Name = "المحقق المسؤول")]
        public string? AssignedInvestigatorId { get; set; }

        [Display(Name = "ملاحظات")]
        [StringLength(500, ErrorMessage = "الملاحظات يجب أن تكون أقل من 500 حرف")]
        public string? Notes { get; set; }

        // Available options
        public List<Report> AvailableReports { get; set; } = new List<Report>();
        public List<ApplicationUser> AvailableInvestigators { get; set; } = new List<ApplicationUser>();
    }

    public class CaseListViewModel
    {
        public List<CaseViewModel> Cases { get; set; } = new List<CaseViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public CaseStatus? FilterStatus { get; set; }
        public string? FilterInvestigatorId { get; set; }
        public DateTime? FilterDateFrom { get; set; }
        public DateTime? FilterDateTo { get; set; }
        public List<ApplicationUser> AvailableInvestigators { get; set; } = new List<ApplicationUser>();
    }

    public class CaseDetailsViewModel
    {
        public CaseViewModel Case { get; set; } = new CaseViewModel();
        public ReportViewModel Report { get; set; } = new ReportViewModel();
        public List<SuspectViewModel> Suspects { get; set; } = new List<SuspectViewModel>();
        public List<DocumentViewModel> Documents { get; set; } = new List<DocumentViewModel>();
        public List<CaseUpdateViewModel> Updates { get; set; } = new List<CaseUpdateViewModel>();
        public List<ApplicationUser> AvailableInvestigators { get; set; } = new List<ApplicationUser>();
    }

    public class CaseUpdateViewModel
    {
        public int Id { get; set; }
        public string UpdateText { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public UpdateType Type { get; set; }
        public string? Notes { get; set; }
        public string UpdatedByFullName { get; set; } = string.Empty;
    }
}