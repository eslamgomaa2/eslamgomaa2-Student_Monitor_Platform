using Core.Enums;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentBehaviorPlatform.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .CountAsync();
        }

        public async Task<int> GetMonitoredStudentsCountTodayAsync()
        {
            var today = DateTime.Today;
            return await _context.AttendanceRecords
                .Where(a => a.AttendanceDate == today)
                .Select(a => a.StudentID)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetStudentsImprovingCountAsync()
        {
            // Students improving = students in cluster group 0 (typically the best performing group)
            return await _context.ClusterMembers
                .Where(cm => cm.ClusterGroup != null && cm.ClusterGroup.GroupLabel == "high potential")
                .Select(cm => cm.StudentID)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetStudentsAtRiskCountAsync()
        {
            // Students at risk = students in higher numbered cluster groups (typically lower performing)
            return await _context.ClusterMembers
                .Where(cm => cm.ClusterGroup != null && cm.ClusterGroup.GroupLabel == "at-risk")
                .Select(cm => cm.StudentID)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetPositiveBehaviorsCountAsync()
        {
            // Positive behaviors = incidents with positive/low severity rules
            return await _context.BehaviorIncidents
                .Include(bi => bi.BehaviorRule)
                .Where(bi => bi.BehaviorRule != null && bi.BehaviorRule.SeverityLevel <= 2)
                .CountAsync();
        }

        public async Task<int> GetBehavioralIssuesCountAsync()
        {
            // Behavioral issues = incidents with high severity
            return await _context.BehaviorIncidents
                .Include(bi => bi.BehaviorRule)
                .Where(bi => bi.BehaviorRule != null && bi.BehaviorRule.SeverityLevel > 2)
                .CountAsync();
        }

        public async Task<int> GetHonorRollCountAsync()
        {
            // Honor roll = students with average GPA >= 3.5
            var honorRollStudents = new List<int>();
            var students = await _context.Students.ToListAsync();

            foreach (var student in students)
            {
                var avgGpa = await GetStudentAverageGPAAsync(student.StudentID);
                if (avgGpa >= 3.5m)
                {
                    honorRollStudents.Add(student.StudentID);
                }
            }

            return honorRollStudents.Count;
        }

        

        public async Task<decimal> GetStudentAverageGPAAsync(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentID == studentId)
                .ToListAsync();

            if (grades.Count == 0)
                return 0m;

            // Convert numerical scores to GPA scale (0-4.0)
            var gpaScores = grades.Select(g => ConvertScoreToGPA(g.Score)).ToList();
            return gpaScores.Count > 0 ? gpaScores.Average() : 0m;
        }

       /* public async Task<int> GetStudentAssessmentCompletedAsync(int studentId)
        {
            // Assuming assessment completion is tracked through StudentNotes with specific type
            return await _context.assignmentSubmissions
                .Where(sn => sn.StudentID == studentId && sn.NoteType == "Assessment")
                .CountAsync();
        }*/

        public async Task<int> GetStudentAssignmentSubmittedAsync(int studentId)
        {
            return await _context.assignmentSubmissions
                .Where(sn => sn.StudentId == studentId)
                .CountAsync();
        }

        public async Task<int> GetStudentReadingWordsAsync(int studentId)
        {
            // Assuming reading words count is tracked in StudentNotes with specific type and contains the count
            var readingNotes = await _context.StudentNotes
                .Where(sn => sn.StudentID == studentId && sn.NoteType == "Reading")
                .ToListAsync();

            int totalWords = 0;
            foreach (var note in readingNotes)
            {
                if (note.NoteText != null && int.TryParse(note.NoteText.Split(':').LastOrDefault()?.Trim(), out int words))
                {
                    totalWords += words;
                }
            }

            return totalWords;
        }

        public async Task<List<(string Subject, decimal Score, string GradeLabel)>> GetStudentTopThreeSubjectsAsync(int studentId)
        {
            var topThree = await _context.Grades
                .Where(g => g.StudentID == studentId)
                .OrderByDescending(g => g.Score)
                .Take(3)
                .Select(g => new { g.Subject, g.Score, g.GradeLabel })
                .ToListAsync();

            return topThree
                .Select(g => (g.Subject ?? "Unknown", g.Score, g.GradeLabel ?? "N/A"))
                .ToList();
        }
        public async Task<int> GetPresentStudentsCountAsync(DateOnly? date = null)
        {
            var targetDate = (date ?? DateOnly.FromDateTime(DateTime.Today))
                                 .ToDateTime(TimeOnly.MinValue);

            return await _context.AttendanceRecords
                .Where(a => a.AttendanceDate.Date == targetDate.Date
                         && a.Status == AttendanceStatus.Present)
                .Select(a => a.StudentID)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetLateStudentsCountAsync(DateOnly? date = null)
        {
            var targetDate = (date ?? DateOnly.FromDateTime(DateTime.Today))
                                 .ToDateTime(TimeOnly.MinValue);

            return await _context.AttendanceRecords
                .Where(a => a.AttendanceDate.Date == targetDate.Date
                         && a.Status == AttendanceStatus.Late)
                .Select(a => a.StudentID)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetAbsentStudentsCountAsync(DateOnly? date = null)
        {
            var targetDate = (date ?? DateOnly.FromDateTime(DateTime.Today))
                                 .ToDateTime(TimeOnly.MinValue);

            return await _context.AttendanceRecords
                .Where(a => a.AttendanceDate.Date == targetDate.Date
                         && a.Status == AttendanceStatus.Absent)
                .Select(a => a.StudentID)
                .Distinct()
                .CountAsync();
        }

        private static decimal ConvertScoreToGPA(decimal score)
        {
            // Convert 0-100 scale to 0-4.0 GPA scale
            return (score / 100) * 4.0m;
        }
    }
}