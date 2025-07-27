using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models.Entities
{
    public class CaseSuspect
    {
        [Key]
        public int Id { get; set; }

        public int CaseId { get; set; }
        public virtual Case Case { get; set; } = null!;

        public int SuspectId { get; set; }
        public virtual Suspect Suspect { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Notes { get; set; }
    }
}