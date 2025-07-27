using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ReporterName { get; set; }

        [Required]
        [StringLength(20)]
        public string ReporterIdNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ReportType { get; set; } // Theft, Assault, Fraud, etc.

        [Required]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public string? Attachments { get; set; } // Comma-separated file paths

        public string Status { get; set; } = "Pending"; // Pending, UnderInvestigation, Closed, Transferred

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<Suspect> Suspects { get; set; }
    }
}