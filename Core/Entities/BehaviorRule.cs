using System;
using System.Collections.Generic;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class BehaviorRule
    {
        public int RuleID { get; set; }
        public string? RuleName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int SeverityLevel { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ApplicationUser? CreatedByUser { get; set; }
        public ICollection<BehaviorIncident> BehaviorIncidents { get; set; } = [];
    }
}