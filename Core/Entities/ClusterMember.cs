namespace StudentBehaviorPlatform.Data.Entities
{
    public class ClusterMember
    {
        public int MemberID { get; set; }
        public int RunID { get; set; }
        public int GroupID { get; set; }
        public int StudentID { get; set; }
        public string? Features { get; set; }

        // Navigation
        public ClusterRun? ClusterRun { get; set; }
        public ClusterGroup? ClusterGroup { get; set; }
        public Student? Student { get; set; }
    }
}