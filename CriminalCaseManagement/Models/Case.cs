using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models
{
    public enum CaseStatus
    {
        UnderInvestigation = 1,
        Closed = 2,
        TransferredToProsecution = 3,
        Suspended = 4
    }

    public class Case
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CaseNumber { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? ClosedDate { get; set; }
        
        [Required]
        public CaseStatus Status { get; set; } = CaseStatus.UnderInvestigation;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Foreign Keys
        public int ReportId { get; set; }
        public int AssignedInvestigatorId { get; set; }
        
        // Navigation Properties
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }
        
        [ForeignKey("AssignedInvestigatorId")]
        public virtual User AssignedInvestigator { get; set; }
        
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual ICollection<SuspectCase> SuspectCases { get; set; } = new List<SuspectCase>();
    }
}