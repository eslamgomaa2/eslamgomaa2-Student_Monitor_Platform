using E_Learning.Core.Base;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class AdminProfile : BaseEntity
    {
        public int AppUserId { get; set; }
        public string? ProfilePicture { get; set; }
        public bool EmailNotificationsEnabled { get; set; } = true;
        public bool PushNotificationsEnabled { get; set; } = true;
        public string Language { get; set; } = "en";
       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser AppUser { get; set; } = null!;
    }
}
