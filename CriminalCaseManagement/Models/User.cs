using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models
{
    public enum UserRole
    {
        SystemAdmin = 1,
        Investigator = 2,
        ReportWriter = 3
    }

    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }
        
        [Required]
        public UserRole Role { get; set; }
        
        [StringLength(20)]
        public string? Rank { get; set; }
        
        [StringLength(100)]
        public string? Department { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual ICollection<Case> AssignedCases { get; set; } = new List<Case>();
        public virtual ICollection<Report> CreatedReports { get; set; } = new List<Report>();
    }
}