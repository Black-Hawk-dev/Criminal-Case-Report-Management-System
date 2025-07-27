using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models.Entities
{
    public class ReportSuspect
    {
        [Key]
        public int Id { get; set; }

        public int ReportId { get; set; }
        public virtual Report Report { get; set; } = null!;

        public int SuspectId { get; set; }
        public virtual Suspect Suspect { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Notes { get; set; }
    }
}