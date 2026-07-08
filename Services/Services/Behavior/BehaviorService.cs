using Core.Enums;
using Core.Interfaces.Repositories;
using E_Learning.Core.Base;
using Services.Dtos.BehaviorRecognation;
using Services.Services.BehaviorRecognition;
using Services.Services.FaceRecognition;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Behavior
{
    public class BehaviorService : IBehaviorService
    {
        private readonly IBehaviorRecognitionService _behaviorRecognition;
        private readonly IFaceRecognitionService _faceRecognition;

        private readonly IBehaviorIncidentRepo _behaviorRepo;

        private readonly ResponseHandler _responseHandler;


        private readonly IUnitOfWork _unitOfWork;

        public BehaviorService(
            IBehaviorRecognitionService behaviorRecognition,
            IUnitOfWork unitOfWork,
            IFaceRecognitionService faceRecognition,
            ResponseHandler responseHandler,
            IBehaviorIncidentRepo behaviorRepo)
        {
            _behaviorRecognition = behaviorRecognition;
            _unitOfWork = unitOfWork;
            _faceRecognition = faceRecognition;
            _responseHandler = responseHandler;
            _behaviorRepo = behaviorRepo;
        }

        public async Task<List<BehaviorIncident>> DetectAndSaveBehaviorAsync(Stream imageStream, CancellationToken ct = default)
        {
            using var imageBytes = new MemoryStream();
            await imageStream.CopyToAsync(imageBytes, ct);

            // Reset position so we can clone the stream for both services
            imageBytes.Position = 0;
            var faceStream = new MemoryStream(imageBytes.ToArray());
            var behaviorStream = new MemoryStream(imageBytes.ToArray());

            var student = await _faceRecognition.RecognizeFaceAsync(faceStream, ct);
            if (student == null || !student.IsRecognized)
                throw new InvalidOperationException(" Face not recognized!");

            var studentEntity = await _unitOfWork.Students.GetByIdAsync(student.StudentId);
            if (studentEntity == null)
                throw new InvalidOperationException($"Student '{student.StudentId}' not found!");

            var incidents = await _behaviorRecognition.RecognizeBehaviorAsync(behaviorStream, ct);
            if (incidents?.Detections == null || incidents.Detections.Count == 0)
                return new List<BehaviorIncident>();

            // 🔑 Deduplicate: one incident per unique behavior, keeping the highest confidence detection
            var distinctDetections = incidents.Detections
                .GroupBy(d => d.Behavior)
                .Select(g => g.OrderByDescending(d => d.Confidence).First())
                .ToList();

            var behaviorIncidents = new List<BehaviorIncident>();

            foreach (var detection in distinctDetections)
            {
                var rule = await _unitOfWork.BehaviorRules.GetByBehaviorNameAsync(detection.Behavior);
                if (rule == null)
                {
                    Console.WriteLine($"⚠️ No rule found for: {detection.Behavior}");
                    continue;
                }

                behaviorIncidents.Add(new BehaviorIncident
                {
                    StudentID = student.StudentId,
                    RuleID = rule.RuleID,
                    Source = "AI",
                    Detail = $"Detected behavior: {rule.Description}",
                    Confidence = detection.Confidence,
                    OccurredAt = DateTime.UtcNow,
                    ReviewStatus = ReviewStatus.Pending
                });
            }

            await _unitOfWork.BehaviorIncidents.AddRangeAsync(behaviorIncidents);
            await _unitOfWork.SaveChangesAsync();

            Console.WriteLine($"✅ Saved {behaviorIncidents.Count} incident(s) for student {student.StudentId}");
            return behaviorIncidents;
        }


        public async Task<Response<IEnumerable<BehaviorIncidentDto>>> GetAllBehaviorIncidentsAsync()
        {
            var incidents = await _behaviorRepo.GetAllWithDetailsAsync();

            if (incidents == null || !incidents.Any())
                return _responseHandler.NotFound<IEnumerable<BehaviorIncidentDto>>("No behavior incidents found.");

            var dtos = incidents.Select(MapToDto);
            return _responseHandler.Success(dtos);
        }

        public async Task<Response<IEnumerable<BehaviorIncidentDto>>> GetBehaviorByStudentAsync(int studentId)
        {
            // Validation
            if (studentId <= 0)
                return _responseHandler.BadRequest<IEnumerable<BehaviorIncidentDto>>("Invalid Student ID.");

            var incidents = await _behaviorRepo.GetByStudentIdAsync(studentId);

            if (incidents == null || !incidents.Any())
                return _responseHandler.NotFound<IEnumerable<BehaviorIncidentDto>>($"No behavior incidents found for student ID {studentId}.");

            var dtos = incidents.Select(MapToDto);
            return _responseHandler.Success(dtos);
        }

        public async Task<Response<IEnumerable<BehaviorStudentSummaryDto>>> GetBehaviorSummaryAsync()
        {
            var incidents = await _behaviorRepo.GetAllForSummaryAsync();

            if (incidents == null || !incidents.Any())
                return _responseHandler.NotFound<IEnumerable<BehaviorStudentSummaryDto>>("No behavior data available for summary.");

            var summary = incidents
                .GroupBy(b => new
                {
                    b.StudentID,
                    StudentName = b.Student != null ? b.Student.FullName : "Unknown"
                })
                .Select(g => new BehaviorStudentSummaryDto
                {
                    StudentID = g.Key.StudentID,
                    StudentName = g.Key.StudentName,
                    TotalIncidents = g.Count(),
                    PendingIncidents = g.Count(b => b.ReviewStatus == ReviewStatus.Pending),
                    UnderReviewIncidents = g.Count(b => b.ReviewStatus == ReviewStatus.UnderReview),
                    ConfirmedIncidents = g.Count(b => b.ReviewStatus == ReviewStatus.Confirmed),
                    RejectedIncidents = g.Count(b => b.ReviewStatus == ReviewStatus.Rejected),
                    LastIncidentDate = g.Max(b => b.OccurredAt)
                });

            return _responseHandler.Success(summary);
        }

        // ============================================================
        // PRIVATE MAPPER
        // ============================================================
        private static BehaviorIncidentDto MapToDto(StudentBehaviorPlatform.Data.Entities.BehaviorIncident b)
        {
            return new BehaviorIncidentDto
            {
                IncidentID = b.IncidentID,
                StudentID = b.StudentID,
                StudentName = b.Student != null ? b.Student.FullName : "Unknown",
                RuleID = b.RuleID,
                RuleName = b.BehaviorRule != null ? b.BehaviorRule.RuleName : null,
                Source = b.Source,
                Detail = b.Detail,
                Confidence = b.Confidence,
                OccurredAt = b.OccurredAt,
                ReviewedByUserID = b.ReviewedByUserID,
                ReviewedByUserName = b.ReviewedByUser != null ? b.ReviewedByUser.UserName : null,
                ReviewStatus = b.ReviewStatus
            };
        }
    }
}
