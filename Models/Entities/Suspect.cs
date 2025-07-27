using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriminalCaseManagement.Models.Entities
{
    public class Suspect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string IdNumber { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? Nationality { get; set; }

        public SuspectStatus Status { get; set; } = SuspectStatus.Active;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties - Many-to-Many relationships
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }

    public enum SuspectStatus
    {
        Active,         // نشط
        Arrested,       // مقبوض عليه
        Released,       // مطلق سراحه
        Wanted,         // مطلوب
        Deceased        // متوفى
    }
}