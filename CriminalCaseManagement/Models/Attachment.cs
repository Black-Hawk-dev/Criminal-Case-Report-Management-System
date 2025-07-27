namespace CriminalCaseManagement.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }
        
        [StringLength(100)]
        public string? FileType { get; set; }
        
        public long FileSize { get; set; }
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        
        // Foreign Keys (nullable for polymorphic relationship)
        public int? ReportId { get; set; }
        public int? CaseId { get; set; }
        
        // Navigation Properties
        [ForeignKey("ReportId")]
        public virtual Report? Report { get; set; }
        
        [ForeignKey("CaseId")]
        public virtual Case? Case { get; set; }
    }
}
