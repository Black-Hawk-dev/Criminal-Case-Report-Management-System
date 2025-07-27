namespace CriminalCaseManagement.Models
{
    public enum ReportType
    {
        Theft = 1,
        Assault = 2,
        Fraud = 3,
        Murder = 4,
        Robbery = 5,
        Other = 6
    }

    public class Report
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ReporterName { get; set; }
        
        [Required]
        [StringLength(20)]
        public string ReporterIdNumber { get; set; }
        
        [Required]
        public ReportType Type { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        
        public DateTime ReportDate { get; set; } = DateTime.Now;
        
        [StringLength(200)]
        public string? Location { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Foreign Keys
        public int CreatedByUserId { get; set; }
        
        // Navigation Properties
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedBy { get; set; }
        
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual ICollection<SuspectReport> SuspectReports { get; set; } = new List<SuspectReport>();
    }
}
