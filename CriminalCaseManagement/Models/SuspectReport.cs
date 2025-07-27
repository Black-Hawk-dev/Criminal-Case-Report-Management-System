namespace CriminalCaseManagement.Models
{
    public class SuspectReport
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public int SuspectId { get; set; }
        public int ReportId { get; set; }
        
        public DateTime AssociatedDate { get; set; } = DateTime.Now;
        
        public string? Notes { get; set; }
        
        // Navigation Properties
        [ForeignKey("SuspectId")]
        public virtual Suspect Suspect { get; set; }
        
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }
    }
}
