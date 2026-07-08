namespace StudentBehaviorPlatform.Data.Entities
{
    public class Grade
    {
        public int GradeID { get; set; }
        public int StudentID { get; set; }
        public string? Subject { get; set; }
        public decimal Score { get; set; }
        public string? GradeLabel { get; set; }
        public string? Term { get; set; }
        public int AcademicYear { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Navigation
        public Student? Student { get; set; }
    }
}