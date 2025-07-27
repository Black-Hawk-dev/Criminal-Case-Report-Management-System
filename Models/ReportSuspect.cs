namespace CriminalCaseManagement.Models
{
    public class ReportSuspect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public int SuspectId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string Notes { get; set; }

        // Navigation Properties
        public virtual Report Report { get; set; }
        public virtual Suspect Suspect { get; set; }
    }
}
