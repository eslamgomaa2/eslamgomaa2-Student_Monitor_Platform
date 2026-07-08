using E_Learning.Core.Base;
using StudentBehaviorPlatform.Data.Entities;

namespace E_Learning.Core.Entities.Assessments.Assignments
{
    public class AssignmentSubmission : BaseEntity
    {
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public DateTime? SubmittedAt { get; set; }
        public string? FileUrl { get; set; }
        public string? Notes { get; set; }
        public decimal? Score { get; set; }
        public string? TeacherComment { get; set; }

     
    }
}
