using System.ComponentModel.DataAnnotations;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "اسم الملف")]
        public string FileName { get; set; } = string.Empty;

        [Display(Name = "الاسم الأصلي")]
        public string OriginalFileName { get; set; } = string.Empty;

        [Display(Name = "نوع الملف")]
        public string ContentType { get; set; } = string.Empty;

        [Display(Name = "حجم الملف")]
        public long FileSize { get; set; }

        [Display(Name = "وصف الملف")]
        public string? Description { get; set; }

        [Display(Name = "نوع المستند")]
        public DocumentType Type { get; set; }

        [Display(Name = "تاريخ الرفع")]
        public DateTime UploadedAt { get; set; }

        [Display(Name = "تم الرفع بواسطة")]
        public string UploadedByFullName { get; set; } = string.Empty;

        // Related data
        public int? ReportId { get; set; }
        public int? CaseId { get; set; }

        // Helper properties
        public string FileSizeFormatted => FormatFileSize(FileSize);
        public string FileExtension => Path.GetExtension(OriginalFileName).ToLowerInvariant();
        public bool IsImage => ContentType.StartsWith("image/");
        public bool IsPdf => ContentType == "application/pdf";
        public bool IsVideo => ContentType.StartsWith("video/");
        public bool IsAudio => ContentType.StartsWith("audio/");
    }

    public class UploadDocumentViewModel
    {
        [Required(ErrorMessage = "الملف مطلوب")]
        [Display(Name = "الملف")]
        public IFormFile File { get; set; } = null!;

        [Display(Name = "وصف الملف")]
        [StringLength(200, ErrorMessage = "الوصف يجب أن يكون أقل من 200 حرف")]
        public string? Description { get; set; }

        [Display(Name = "نوع المستند")]
        public DocumentType Type { get; set; } = DocumentType.Other;

        // Related entity
        public int? ReportId { get; set; }
        public int? CaseId { get; set; }
    }

    public class DocumentListViewModel
    {
        public List<DocumentViewModel> Documents { get; set; } = new List<DocumentViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public DocumentType? FilterType { get; set; }
        public DateTime? FilterDateFrom { get; set; }
        public DateTime? FilterDateTo { get; set; }
        public int? FilterReportId { get; set; }
        public int? FilterCaseId { get; set; }
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}