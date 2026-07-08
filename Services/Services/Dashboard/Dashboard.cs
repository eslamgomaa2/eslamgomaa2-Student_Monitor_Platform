using Core.Interfaces.Repositories;
using E_Learning.Core.Base;
using Services.Dtos.Dashboard;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;

namespace Services.Services.Dashboard
{
    public class Dashboard : IDashboard
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitofwork;


        public Dashboard(IDashboardRepository dashboardRepository, ResponseHandler responseHandler, IUnitOfWork unitofwork)
        {
            _dashboardRepository = dashboardRepository;
            _responseHandler = responseHandler;
            _unitofwork = unitofwork;
        }


        public async Task<Response<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            try
            {
                var data = new DashboardStatsDto
                {
                    TotalStudents = await _dashboardRepository.GetTotalStudentsCountAsync(),
                    StudentsMonitoreditToday = await _dashboardRepository.GetMonitoredStudentsCountTodayAsync(),
                    StudentsImproving = await _dashboardRepository.GetStudentsImprovingCountAsync(),
                    StudentsAtRisk = await _dashboardRepository.GetStudentsAtRiskCountAsync(),
                    PositiveBehaviors = await _dashboardRepository.GetPositiveBehaviorsCountAsync(),
                    BehavioralIssues = await _dashboardRepository.GetBehavioralIssuesCountAsync(),
                    HonorRoll = await _dashboardRepository.GetHonorRollCountAsync(),
                    PresentStudents = await _dashboardRepository.GetPresentStudentsCountAsync(),
                    LateStudents = await _dashboardRepository.GetLateStudentsCountAsync(),
                    AbsentStudents = await _dashboardRepository.GetAbsentStudentsCountAsync()
                };

                return _responseHandler.Success(data);
            }
            catch (Exception ex)
            {
                return _responseHandler.BadRequest<DashboardStatsDto>(ex.Message);
            }
        }

        public async Task<Response<StudentAcademicDto>> GetStudentAcademicDataAsync(int studentId)
        {
            try
            {
                var studentexist = await _unitofwork.Students.GetByIdAsync(studentId);
                if (studentexist is null) 
                {
                    return _responseHandler.NotFound<StudentAcademicDto>("Student Not Fount");
                }
                var averageGPA = await _dashboardRepository.GetStudentAverageGPAAsync(studentId);
                

                if (averageGPA == null)
                    return _responseHandler.NotFound<StudentAcademicDto>("Student not found");
/*
                var assessmentCompleted = await _dashboardRepository.GetStudentAssessmentCompletedAsync(studentId);*/
                var assignmentSubmitted = await _dashboardRepository.GetStudentAssignmentSubmittedAsync(studentId);
                var readingWords = await _dashboardRepository.GetStudentReadingWordsAsync(studentId);
                var topThreeSubjects = await _dashboardRepository.GetStudentTopThreeSubjectsAsync(studentId);

                var data = new StudentAcademicDto
                {
                    StudentID = studentId,
                    FullName = studentexist.FullName,
                    AverageGPA = averageGPA,/*
                    AssessmentCompleted = assessmentCompleted,*/
                    AssignmentSubmitted = assignmentSubmitted,
                    ReadingWords = readingWords,
                    TopThreeSubjects = topThreeSubjects
                        .Select(t => new StudentTopSubjectDto
                        {
                            Subject = t.Subject,
                            Score = t.Score,
                            GradeLabel = t.GradeLabel
                        })
                        .ToList()
                };

                return _responseHandler.Success(data);
            }
            catch (Exception ex)
            {
                return _responseHandler.BadRequest<StudentAcademicDto>(ex.Message);
            }
        }

        public async Task<Response<int>> GetPresentStudentCountAsync(DateOnly? date = null)
        {
            try
            {
                var count = await _dashboardRepository.GetPresentStudentsCountAsync(date);
                return _responseHandler.Success(count);
            }
            catch (Exception ex)
            {
                return _responseHandler.BadRequest<int>(ex.Message);
            }
        }

        public async Task<Response<int>> GetAbsentStudentCountAsync(DateOnly? date = null)
        {
            try
            {
                var count = await _dashboardRepository.GetAbsentStudentsCountAsync(date);
                return _responseHandler.Success(count);
            }
            catch (Exception ex)
            {
                return _responseHandler.BadRequest<int>(ex.Message);
            }
        }

        public async Task<Response<int>> GetLateStudentCountAsync(DateOnly? date = null)
        {
            try
            {
                var count = await _dashboardRepository.GetLateStudentsCountAsync(date);
                return _responseHandler.Success(count);
            }
            catch (Exception ex)
            {
                return _responseHandler.BadRequest<int>(ex.Message);
            }
        }
    }
}