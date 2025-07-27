namespace CriminalCaseManagement.Models
{
    public class Suspect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string IdNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Nationality { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Arrested, Released, Convicted

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
    }
}
