using Microsoft.AspNetCore.Identity;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Seeding
{
    public static class AdminSeeding
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, AppDbContext context)
        {

            const string adminEmail = "principal@school.edu";
            const string adminPassword = "Admin123!";

            var adminExists = await userManager.FindByEmailAsync(adminEmail);
            if (adminExists is null)
            {
                var admin = new ApplicationUser
                {
                    FullName = "System Admin",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,

                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Teacher");


                    var adminProfile = new AdminProfile
                    {
                        AppUserId = admin.Id,
                        CreatedAt = DateTime.UtcNow
                    };
                    await context.Set<AdminProfile>().AddAsync(adminProfile);

                    await context.SaveChangesAsync();
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }

            }
        }

    }
}
