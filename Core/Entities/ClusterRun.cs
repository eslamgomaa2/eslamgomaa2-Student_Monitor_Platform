using System;
using System.Collections.Generic;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class ClusterRun
    {
        public int RunID { get; set; }
        public int TriggeredByUserID { get; set; }
        public string? FiltersApplied { get; set; }
        public int NumClusters { get; set; }
        public DateTime RunAt { get; set; } = DateTime.UtcNow;
        public string? SchoolYear { get; set; }
        public string? GradeLevel { get; set; }
        public string? ReportPath { get; set; }

        // Navigation
        public ApplicationUser? TriggeredByUser { get; set; }
        public ICollection<ClusterGroup> ClusterGroups { get; set; } = [];
        public ICollection<ClusterMember> ClusterMembers { get; set; } = [];
    }
}