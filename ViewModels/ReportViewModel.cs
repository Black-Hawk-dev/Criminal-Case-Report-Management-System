namespace CriminalCaseManagement.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المبلغ مطلوب")]
        [StringLength(100, ErrorMessage = "اسم المبلغ يجب أن يكون أقل من 100 حرف")]
        [Display(Name = "اسم المبلغ")]
        public string ReporterName { get; set; }

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [StringLength(20, ErrorMessage = "رقم الهوية يجب أن يكون أقل من 20 حرف")]
        [Display(Name = "رقم الهوية")]
        public string ReporterIdNumber { get; set; }

        [Required(ErrorMessage = "نوع البلاغ مطلوب")]
        [StringLength(50, ErrorMessage = "نوع البلاغ يجب أن يكون أقل من 50 حرف")]
        [Display(Name = "نوع البلاغ")]
        public string ReportType { get; set; }

        [Required(ErrorMessage = "تاريخ البلاغ مطلوب")]
        [Display(Name = "تاريخ البلاغ")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "وصف البلاغ مطلوب")]
        [StringLength(500, ErrorMessage = "وصف البلاغ يجب أن يكون أقل من 500 حرف")]
        [Display(Name = "وصف البلاغ")]
        public string Description { get; set; }

        [StringLength(200, ErrorMessage = "الموقع يجب أن يكون أقل من 200 حرف")]
        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Display(Name = "المرفقات")]
        public IFormFileCollection? Attachments { get; set; }

        public string Status { get; set; } = "Pending";

        public List<string> AvailableReportTypes { get; set; } = new List<string>
        {
            "سرقة", "اعتداء", "احتيال", "تهديد", "إتلاف ممتلكات", "مخدرات", "أخرى"
        };
    }

    public class ReportListViewModel
    {
        public List<ReportViewModel> Reports { get; set; } = new List<ReportViewModel>();
        public string SearchTerm { get; set; }
        public string StatusFilter { get; set; }
        public string TypeFilter { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
