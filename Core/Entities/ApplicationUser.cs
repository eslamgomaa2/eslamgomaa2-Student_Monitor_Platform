using Microsoft.AspNetCore.Identity;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<VideoSession> VideoSessions { get; set; } = [];
        public AdminProfile? AdminProfile { get; set; } 

        public ICollection<StudentNote> StudentNotes { get; set; } = [];
        public ICollection<BehaviorRule> BehaviorRules { get; set; } = [];
        public ICollection<BehaviorIncident> ReviewedBehaviorIncidents { get; set; } = [];
        public ICollection<ClusterRun> ClusterRuns { get; set; } = [];
    }
}