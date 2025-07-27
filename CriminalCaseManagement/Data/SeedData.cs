using Microsoft.AspNetCore.Identity;
using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they don't exist
            string[] roleNames = { "SystemAdmin", "Investigator", "ReportWriter" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user if it doesn't exist
            var adminEmail = "admin@criminalcase.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "مدير النظام",
                    Role = UserRole.SystemAdmin,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
                }
            }

            // Create sample investigator
            var investigatorEmail = "investigator@criminalcase.com";
            var investigatorUser = await userManager.FindByEmailAsync(investigatorEmail);

            if (investigatorUser == null)
            {
                investigatorUser = new ApplicationUser
                {
                    UserName = investigatorEmail,
                    Email = investigatorEmail,
                    FullName = "أحمد محمد المحقق",
                    Role = UserRole.Investigator,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(investigatorUser, "Investigator123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(investigatorUser, "Investigator");
                }
            }

            // Create sample report writer
            var writerEmail = "writer@criminalcase.com";
            var writerUser = await userManager.FindByEmailAsync(writerEmail);

            if (writerUser == null)
            {
                writerUser = new ApplicationUser
                {
                    UserName = writerEmail,
                    Email = writerEmail,
                    FullName = "فاطمة علي كاتبة البلاغات",
                    Role = UserRole.ReportWriter,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(writerUser, "Writer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(writerUser, "ReportWriter");
                }
            }

            // Save changes
            await context.SaveChangesAsync();
        }
    }
}