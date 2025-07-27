using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المبلغ مطلوب")]
        [Display(Name = "اسم المبلغ")]
        [StringLength(100, ErrorMessage = "اسم المبلغ يجب أن يكون أقل من 100 حرف")]
        public string ReporterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [Display(Name = "رقم الهوية")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        public string ReporterIdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع البلاغ مطلوب")]
        [Display(Name = "نوع البلاغ")]
        public ReportType Type { get; set; }

        [Required(ErrorMessage = "تاريخ البلاغ مطلوب")]
        [Display(Name = "تاريخ البلاغ")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "وصف البلاغ مطلوب")]
        [Display(Name = "وصف البلاغ")]
        [StringLength(500, ErrorMessage = "وصف البلاغ يجب أن يكون أقل من 500 حرف")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "الموقع")]
        [StringLength(200, ErrorMessage = "الموقع يجب أن يكون أقل من 200 حرف")]
        public string? Location { get; set; }

        [Display(Name = "الحالة")]
        public ReportStatus Status { get; set; } = ReportStatus.New;

        // Navigation properties
        public string CreatedByFullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int RelatedCasesCount { get; set; }
        public int RelatedSuspectsCount { get; set; }
        public int AttachmentsCount { get; set; }
    }

    public class CreateReportViewModel
    {
        [Required(ErrorMessage = "اسم المبلغ مطلوب")]
        [Display(Name = "اسم المبلغ")]
        [StringLength(100, ErrorMessage = "اسم المبلغ يجب أن يكون أقل من 100 حرف")]
        public string ReporterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [Display(Name = "رقم الهوية")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        public string ReporterIdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع البلاغ مطلوب")]
        [Display(Name = "نوع البلاغ")]
        public ReportType Type { get; set; }

        [Required(ErrorMessage = "تاريخ البلاغ مطلوب")]
        [Display(Name = "تاريخ البلاغ")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "وصف البلاغ مطلوب")]
        [Display(Name = "وصف البلاغ")]
        [StringLength(500, ErrorMessage = "وصف البلاغ يجب أن يكون أقل من 500 حرف")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "الموقع")]
        [StringLength(200, ErrorMessage = "الموقع يجب أن يكون أقل من 200 حرف")]
        public string? Location { get; set; }

        [Display(Name = "المرفقات")]
        public List<IFormFile>? Attachments { get; set; }
    }

    public class ReportListViewModel
    {
        public List<ReportViewModel> Reports { get; set; } = new List<ReportViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public ReportType? FilterType { get; set; }
        public ReportStatus? FilterStatus { get; set; }
        public DateTime? FilterDateFrom { get; set; }
        public DateTime? FilterDateTo { get; set; }
    }
}