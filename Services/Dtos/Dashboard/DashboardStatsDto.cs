namespace Services.Dtos.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalStudents { get; set; }
        public int StudentsMonitoreditToday { get; set; }
        public int StudentsImproving { get; set; }
        public int StudentsAtRisk { get; set; }
        public int PositiveBehaviors { get; set; }
        public int BehavioralIssues { get; set; }
        public int HonorRoll { get; set; }
        public int PresentStudents { get; set; }
        public int LateStudents { get; set; }
        public int AbsentStudents { get; set; }
    }
}