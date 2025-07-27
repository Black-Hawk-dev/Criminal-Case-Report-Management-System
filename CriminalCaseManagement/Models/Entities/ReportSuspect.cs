using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models.Entities
{
    public class ReportSuspect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public int SuspectId { get; set; }

        [Display(Name = "تاريخ الإضافة")]
        public DateTime AddedDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        // Navigation Properties
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; } = null!;

        [ForeignKey("SuspectId")]
        public virtual Suspect Suspect { get; set; } = null!;
    }
}