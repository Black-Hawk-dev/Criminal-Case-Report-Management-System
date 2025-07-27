using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class CaseViewModel
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public CaseStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AssignedInvestigatorName { get; set; }
        public string? ReportTitle { get; set; }
        public int SuspectsCount { get; set; }
        public int DocumentsCount { get; set; }
        public int UpdatesCount { get; set; }
    }

    public class CreateCaseViewModel
    {
        [Required(ErrorMessage = "رقم القضية مطلوب")]
        [StringLength(50, ErrorMessage = "رقم القضية يجب أن يكون أقل من 50 حرف")]
        [Display(Name = "رقم القضية")]
        public string CaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "عنوان القضية مطلوب")]
        [StringLength(200, ErrorMessage = "عنوان القضية يجب أن يكون أقل من 200 حرف")]
        [Display(Name = "عنوان القضية")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف القضية مطلوب")]
        [StringLength(1000, ErrorMessage = "وصف القضية يجب أن يكون أقل من 1000 حرف")]
        [Display(Name = "وصف القضية")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ فتح القضية مطلوب")]
        [Display(Name = "تاريخ فتح القضية")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; } = DateTime.Now;

        [Display(Name = "البلاغ المرتبط")]
        public int? ReportId { get; set; }

        [Display(Name = "المحقق المسؤول")]
        public string? AssignedInvestigatorId { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات يجب أن تكون أقل من 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "المرفقات")]
        public List<IFormFile>? Attachments { get; set; }
    }

    public class CaseListViewModel
    {
        public List<CaseViewModel> Cases { get; set; } = new List<CaseViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public CaseStatus? StatusFilter { get; set; }
        public string? InvestigatorFilter { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class CaseDetailsViewModel
    {
        public CaseViewModel Case { get; set; } = new CaseViewModel();
        public List<SuspectViewModel> Suspects { get; set; } = new List<SuspectViewModel>();
        public List<DocumentViewModel> Documents { get; set; } = new List<DocumentViewModel>();
        public List<CaseUpdateViewModel> Updates { get; set; } = new List<CaseUpdateViewModel>();
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