using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models
{
    public class Case
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CaseNumber { get; set; }

        [Required]
        public int ReportId { get; set; }

        public int? InvestigatorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Open"; // Open, UnderInvestigation, Closed, Transferred

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Navigation Properties
        public virtual Report Report { get; set; }
        public virtual Investigator Investigator { get; set; }
        public virtual ICollection<Suspect> Suspects { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}