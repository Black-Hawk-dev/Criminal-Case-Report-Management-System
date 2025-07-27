using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models
{
    public class CaseSuspect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [Required]
        public int SuspectId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string Notes { get; set; }

        // Navigation Properties
        public virtual Case Case { get; set; }
        public virtual Suspect Suspect { get; set; }
    }
}