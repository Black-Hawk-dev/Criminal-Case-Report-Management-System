using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CriminalCaseManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Investigator, Reporter

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}