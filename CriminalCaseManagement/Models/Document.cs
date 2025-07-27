namespace CriminalCaseManagement.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        [StringLength(100)]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int? CaseId { get; set; }
        public int? ReportId { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string UploadedBy { get; set; }

        // Navigation Properties
        public virtual Case Case { get; set; }
        public virtual Report Report { get; set; }
    }
}
