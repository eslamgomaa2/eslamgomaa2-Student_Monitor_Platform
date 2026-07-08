using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Seeding
{
    public static class RoleSeeding
    {
        private static readonly string[] Roles = { "Teacher", "Counselor", "Principal","Staff" };

        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }
    }
}
