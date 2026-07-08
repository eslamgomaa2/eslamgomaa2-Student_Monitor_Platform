using Core.Enums;
using System;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class BehaviorIncident
    {
        public int IncidentID { get; set; }
        public int StudentID { get; set; }
        public int RuleID { get; set; }
        public string? Source { get; set; }
        public string? Detail { get; set; }
        public decimal? Confidence { get; set; }
        public DateTime OccurredAt { get; set; }
        public int? ReviewedByUserID { get; set; }
        public ReviewStatus ReviewStatus { get; set; } = ReviewStatus.Pending;

        // Navigation
        public Student? Student { get; set; }
        public BehaviorRule? BehaviorRule { get; set; }
        public ApplicationUser? ReviewedByUser { get; set; }
    }
}