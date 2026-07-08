using E_Learning.Core.Base;
using StudentBehaviorPlatform.Data.Entities;

namespace E_Learning.Core.Entities.Assessments.Assignments
{
    public class Assignment : AuditableEntity
    {
        public int VideoseasionId { get; set; }
        public VideoSession videoseasion { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<AssignmentSubmission> Submissions { get; set; }
            = new List<AssignmentSubmission>();
    }

}
