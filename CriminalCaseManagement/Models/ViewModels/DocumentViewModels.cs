namespace CriminalCaseManagement.Models.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DocumentType Type { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadedByFullName { get; set; } = string.Empty;
        public string? ReportTitle { get; set; }
        public string? CaseTitle { get; set; }
        public string FileSizeFormatted => FileHelper.FormatFileSize(FileSize);
        public string FileExtension => Path.GetExtension(FileName).ToLowerInvariant();
        public bool IsImage => new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }.Contains(FileExtension);
        public bool IsPdf => FileExtension == ".pdf";
    }

    public class UploadDocumentViewModel
    {
        [Required(ErrorMessage = "الملف مطلوب")]
        [Display(Name = "الملف")]
        public IFormFile File { get; set; } = null!;

        [StringLength(200, ErrorMessage = "الوصف يجب أن يكون أقل من 200 حرف")]
        [Display(Name = "وصف الملف")]
        public string? Description { get; set; }

        [Display(Name = "نوع المستند")]
        public DocumentType Type { get; set; }

        [Display(Name = "البلاغ المرتبط")]
        public int? ReportId { get; set; }

        [Display(Name = "القضية المرتبطة")]
        public int? CaseId { get; set; }
    }

    public class DocumentListViewModel
    {
        public List<DocumentViewModel> Documents { get; set; } = new List<DocumentViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public DocumentType? TypeFilter { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? UploadedByFilter { get; set; }
    }

    public static class FileHelper
    {
        public static string FormatFileSize(long bytes)
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
}
