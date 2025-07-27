
namespace CriminalCaseManagement.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Case> AssignedCases { get; set; } = new List<Case>();
        public virtual ICollection<Report> CreatedReports { get; set; } = new List<Report>();
    }

    public enum UserRole
    {
        SystemAdmin,     // مدير النظام
        Investigator,    // محقق
        ReportWriter     // كاتب بلاغات
    }
}
