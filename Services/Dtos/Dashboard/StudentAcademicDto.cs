namespace Services.Dtos.Dashboard
{
    public class StudentAcademicDto
    {
        public int StudentID { get; set; }
        public string? FullName { get; set; }
        public decimal AverageGPA { get; set; }
        public int AssessmentCompleted { get; set; }
        public int AssignmentSubmitted { get; set; }
        public int ReadingWords { get; set; }
        public List<StudentTopSubjectDto> TopThreeSubjects { get; set; } = new();
    }
}