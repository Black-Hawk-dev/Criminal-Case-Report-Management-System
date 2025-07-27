using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models
{
    public class Investigator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Rank { get; set; } // Lieutenant, Captain, Major, etc.

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(20)]
        public string BadgeNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string? UserId { get; set; }
    }
}