using E_Learning.Repository.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Seeding;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace E_learning.API.Extensions
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILoggerFactory>()
                .CreateLogger("DatabaseMigration");

            try
            {
                logger.LogInformation("Starting migration...");

                var dbContext = services.GetRequiredService<AppDbContext>();

                // Apply Migrations
                  await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migration completed.");
                // 2. Seed roles
                var roleManager = services
                    .GetRequiredService<RoleManager<IdentityRole<int>>>();
                await RoleSeeding.SeedRolesAsync(roleManager);
                logger.LogInformation("Roles seeded successfully");

                // 3. Seed admin + instructor accounts
                var userManager = services
                    .GetRequiredService<UserManager<ApplicationUser>>();
                
                await AdminSeeding.SeedAdminAsync(userManager,dbContext);
                await BehaviorsSedding.SeedBehaviorAsync(dbContext);

                
                logger.LogInformation("Default accounts seeded successfully");


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Migration failed.");
                throw;
            }

            return host;
        }
    }
}