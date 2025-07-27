using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models
{
    public class SuspectCase
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public int SuspectId { get; set; }
        public int CaseId { get; set; }
        
        public DateTime AssociatedDate { get; set; } = DateTime.Now;
        
        public string? Notes { get; set; }
        
        public string? Status { get; set; } // مثل: متهم رئيسي، متهم ثانوي، شاهد
        
        // Navigation Properties
        [ForeignKey("SuspectId")]
        public virtual Suspect Suspect { get; set; }
        
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; }
    }
}