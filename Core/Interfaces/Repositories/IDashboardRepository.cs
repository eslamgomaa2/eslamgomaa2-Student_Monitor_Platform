using StudentBehaviorPlatform.Data.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalStudentsCountAsync();
        Task<int> GetMonitoredStudentsCountTodayAsync();
        Task<int> GetStudentsImprovingCountAsync();
        Task<int> GetStudentsAtRiskCountAsync();
        Task<int> GetPositiveBehaviorsCountAsync();
        Task<int> GetBehavioralIssuesCountAsync();
        Task<int> GetHonorRollCountAsync();
        Task<int> GetPresentStudentsCountAsync(DateOnly? date = null);
        Task<int> GetLateStudentsCountAsync(DateOnly? date = null);
        Task<int> GetAbsentStudentsCountAsync(DateOnly? date = null);
        Task<decimal> GetStudentAverageGPAAsync(int studentId);
       /* Task<int> GetStudentAssessmentCompletedAsync(int studentId);*/
        Task<int> GetStudentAssignmentSubmittedAsync(int studentId);
        Task<int> GetStudentReadingWordsAsync(int studentId);
        Task<List<(string Subject, decimal Score, string GradeLabel)>> GetStudentTopThreeSubjectsAsync(int studentId);
    }
}