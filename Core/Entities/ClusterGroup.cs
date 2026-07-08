using System.Collections.Generic;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class ClusterGroup
    {
        public int GroupID { get; set; }
        public int RunID { get; set; }
        public string GroupLabel { get; set; }
        public string? GroupSummary { get; set; }
        public int StudentCount { get; set; }

        // Navigation
        public ClusterRun? ClusterRun { get; set; }
        public ICollection<ClusterMember> ClusterMembers { get; set; } = [];
    }
}