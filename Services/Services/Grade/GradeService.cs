using Core.Interfaces.Repositories;
using E_Learning.Core.Base;
using Services.Dtos.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Grade
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepo _gradeRepo;
        private readonly ResponseHandler _responseHandler;

        public GradeService(IGradeRepo gradeRepo, ResponseHandler responseHandler)
        {
            _gradeRepo = gradeRepo;
            _responseHandler = responseHandler;
        }

        public async Task<Response<IEnumerable<GradeDto>>> GetAllGradesAsync()
        {
            var grades = await _gradeRepo.GetAllWithStudentAsync();

            if (grades == null || !grades.Any())
                return _responseHandler.NotFound<IEnumerable<GradeDto>>("No grades found.");

            var dtos = grades.Select(MapToDto);
            return _responseHandler.Success(dtos);
        }

        public async Task<Response<IEnumerable<GradeDto>>> GetGradesByStudentAsync(int studentId)
        {
            
            if (studentId <= 0)
                return _responseHandler.BadRequest<IEnumerable<GradeDto>>("Invalid Student ID.");

            var grades = await _gradeRepo.GetByStudentIdAsync(studentId);

            if (grades == null || !grades.Any())
                return _responseHandler.NotFound<IEnumerable<GradeDto>>($"No grades found for student ID {studentId}.");

            var dtos = grades.Select(MapToDto);
            return _responseHandler.Success(dtos);
        }

        public async Task<Response<IEnumerable<GradeDto>>> GetGradesBySubjectAsync(string subject)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(subject))
                return _responseHandler.BadRequest<IEnumerable<GradeDto>>("Subject name cannot be empty.");

            if (subject.Length > 100)
                return _responseHandler.BadRequest<IEnumerable<GradeDto>>("Subject name is too long.");

            var grades = await _gradeRepo.GetBySubjectAsync(subject);

            if (grades == null || !grades.Any())
                return _responseHandler.NotFound<IEnumerable<GradeDto>>($"No grades found for subject '{subject}'.");

            var dtos = grades.Select(MapToDto);
            return _responseHandler.Success(dtos);
        }

        public async Task<Response<IEnumerable<StudentAverageDto>>> GetAveragesAsync()
        {
            var grades = await _gradeRepo.GetAllForAverageAsync();

            if (grades == null || !grades.Any())
                return _responseHandler.NotFound<IEnumerable<StudentAverageDto>>("No grade data available to calculate averages.");

            var averages = grades
                .GroupBy(g => new
                {
                    g.StudentID,
                    StudentName = g.Student != null ? g.Student.FullName : "Unknown"
                })
                .Select(g => new StudentAverageDto
                {
                    StudentID = g.Key.StudentID,
                    StudentName = g.Key.StudentName,
                    AverageScore = Math.Round((double)g.Average(x => x.Score), 1),
                    TotalSubjects = g.Count(),
                    HighestScore = g.Max(x => x.Score),
                    LowestScore = g.Min(x => x.Score)
                });

            return _responseHandler.Success(averages);
        }

        // ============================================================
        // PRIVATE MAPPER
        // ============================================================
        private static GradeDto MapToDto(StudentBehaviorPlatform.Data.Entities.Grade g)
        {
            return new GradeDto
            {
                GradeID = g.GradeID,
                StudentID = g.StudentID,
                StudentName = g.Student != null ? g.Student.FullName : "Unknown",
                Subject = g.Subject,
                Score = g.Score,
                GradeLabel = g.GradeLabel,
                Term = g.Term,
                AcademicYear = g.AcademicYear,
                Date = g.Date
            };
        }
    }
}
