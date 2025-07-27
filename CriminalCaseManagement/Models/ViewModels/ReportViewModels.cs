using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public string ReporterIdNumber { get; set; } = string.Empty;
        public ReportType Type { get; set; }
        public DateTime ReportDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public ReportStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
        public int CasesCount { get; set; }
        public int DocumentsCount { get; set; }
        public int SuspectsCount { get; set; }
    }

    public class CreateReportViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المبلغ مطلوب")]
        [StringLength(100, ErrorMessage = "اسم المبلغ يجب أن يكون أقل من 100 حرف")]
        [Display(Name = "اسم المبلغ")]
        public string ReporterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم هوية المبلغ مطلوب")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        [Display(Name = "رقم هوية المبلغ")]
        public string ReporterIdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع البلاغ مطلوب")]
        [Display(Name = "نوع البلاغ")]
        public ReportType Type { get; set; }

        [Required(ErrorMessage = "تاريخ البلاغ مطلوب")]
        [Display(Name = "تاريخ البلاغ")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "وصف البلاغ مطلوب")]
        [StringLength(500, ErrorMessage = "وصف البلاغ يجب أن يكون أقل من 500 حرف")]
        [Display(Name = "وصف البلاغ")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "الموقع يجب أن يكون أقل من 200 حرف")]
        [Display(Name = "الموقع")]
        public string? Location { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات يجب أن تكون أقل من 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "المرفقات")]
        public List<IFormFile>? Attachments { get; set; }
    }

    public class ReportListViewModel
    {
        public List<ReportViewModel> Reports { get; set; } = new List<ReportViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public ReportType? TypeFilter { get; set; }
        public ReportStatus? StatusFilter { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}