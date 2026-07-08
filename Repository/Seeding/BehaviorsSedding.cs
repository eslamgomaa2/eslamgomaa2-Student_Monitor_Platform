using Microsoft.EntityFrameworkCore;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Seeding
{
    public static class BehaviorsSedding
    {
        static List<string> behaviorNames = new List<string>
        {
            "Looking Forward",
            "Raising Hand",
            "Reading",
            "Sleeping",
            "Turning Around"
        };

        public static async Task SeedBehaviorAsync(AppDbContext context)
        {
            var existingNames = await context.BehaviorRules .Select(b => b.RuleName.ToLower()) .ToListAsync();

            foreach (var rule in behaviorNames)
            {
                if (!existingNames.Contains(rule.ToLower()))
                {
                    var newRule = new BehaviorRule
                    {
                        RuleName = rule,
                        Description = $"This rule detects when a student is {rule}.",
                        Category = "General",
                        SeverityLevel = behaviorNames.IndexOf(rule ) + 1,
                        IsActive = true,
                        CreatedByUserID = 1, 
                        CreatedAt = DateTime.UtcNow
                    };
                    context.BehaviorRules.Add(newRule);
                   

                }else
                {
                    Console.WriteLine($"Behavior rule '{rule}' already exists. Skipping seeding for this rule.");
                }

            }
            await context.SaveChangesAsync();
        }
    }
}

          