namespace CriminalCaseManagement.Models
{
    public class Suspect
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [Required]
        [StringLength(20)]
        public string IdNumber { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }
        
        [StringLength(50)]
        public string? Nationality { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual ICollection<SuspectReport> SuspectReports { get; set; } = new List<SuspectReport>();
        public virtual ICollection<SuspectCase> SuspectCases { get; set; } = new List<SuspectCase>();
    }
}
