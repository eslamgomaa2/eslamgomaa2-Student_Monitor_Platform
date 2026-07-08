using Core.Enums;
using E_Learning.Core.Base;
using Services.Dtos.Attendance;
using Services.Services.FaceRecognition;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using StudentMonitor.Core.Interfaces;
using System.Collections.Generic;

namespace StudentMonitor.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IFaceRecognitionService _faceRecognition;
    private readonly IUnitOfWork unitOfWork;
    private readonly ResponseHandler _responseHandler;



    public AttendanceService(
        IFaceRecognitionService faceRecognition,
        IUnitOfWork _unitOfWork,
        ResponseHandler responseHandler)
    {
        _faceRecognition = faceRecognition;
        unitOfWork = _unitOfWork;
        _responseHandler = responseHandler;
    }

    public async Task<AttendanceRecord> MarkAttendanceAsync(Stream cameraImage, DateTime scheduledTime, int? videoSessionId = null, CancellationToken ct = default)
    {
        var recognitionResult = await _faceRecognition.RecognizeFaceAsync(cameraImage, ct);

        if (!recognitionResult.IsRecognized)
            throw new InvalidOperationException("❌ Face not recognized!");


        var student = await unitOfWork.Students.GetByIdAsync(recognitionResult.StudentId);

        if (student == null)
            throw new InvalidOperationException(
                $"❌ Student with ID '{recognitionResult.StudentId}' not found in database!");


        var existingRecord = await unitOfWork.AttendanceRecords.GetTodayRecordAsync(student.StudentID);

        if (existingRecord != null)
        {


            return existingRecord;
        }




        var now = DateTime.Now;
        var attendanceDate = now.Date;

        AttendanceStatus status;
        int? lateMinutes = null;

        if (now <= scheduledTime)
        {
            status = AttendanceStatus.Present;
        }
        else
        {
            status = AttendanceStatus.Late;
            lateMinutes = (int)(now - scheduledTime).TotalMinutes;
        }

        var record = new AttendanceRecord
        {
            StudentID = student.StudentID,
            VideoSessionID = videoSessionId,
            AttendanceDate = attendanceDate,
            CheckInTime = now,
            CheckOutTime = null,
            Status = status,
            LateMinutes = lateMinutes,
            ConfidenceScore = recognitionResult.Confidence,
            Source = "AI_FaceRecognition",

        };


        await unitOfWork.AttendanceRecords.AddAsync(record);
        await unitOfWork.SaveChangesAsync();


        return record;
    }


    public async Task<Response<IEnumerable<AttendanceRecordDto>>> GetAllAttendanceAsync()
    {
        var records = await unitOfWork.AttendanceRecords.GetAllWithStudentAsync();

        if (records == null || !records.Any())
            return _responseHandler.NotFound<IEnumerable<AttendanceRecordDto>>("No attendance records found.");

        var dtos = records.Select(MapToDto);
        return _responseHandler.Success< IEnumerable < AttendanceRecordDto >> (dtos);
    }

    public async Task<Response<IEnumerable<AttendanceRecordDto>>> GetAttendanceByStudentAsync(int studentId)
    {
        // Validation
        if (studentId <= 0)
            return _responseHandler.BadRequest<IEnumerable<AttendanceRecordDto>>("Invalid Student ID.");

        var records = await unitOfWork.AttendanceRecords.GetByStudentIdAsync(studentId);

        if (records == null || !records.Any())
            return _responseHandler.NotFound<IEnumerable<AttendanceRecordDto>>($"No attendance records found for student ID {studentId}.");

        var dtos = records.Select(MapToDto);
        return _responseHandler.Success(dtos);
    }

    public async Task<Response<IEnumerable<AttendanceRecordDto>>> GetAttendanceByDateAsync(DateTime date)
    {
        // Validation
        if (date == default)
            return _responseHandler.BadRequest<IEnumerable<AttendanceRecordDto>>("Invalid date provided.");

        if (date > DateTime.UtcNow)
            return _responseHandler.BadRequest<IEnumerable<AttendanceRecordDto>>("Date cannot be in the future.");

        var records = await unitOfWork.AttendanceRecords.GetByDateAsync(date);

        if (records == null || !records.Any())
            return _responseHandler.NotFound<IEnumerable<AttendanceRecordDto>>($"No attendance records found for date {date:yyyy-MM-dd}.");

        var dtos = records.Select(MapToDto);
        return _responseHandler.Success(dtos);
    }

    public async Task<Response<IEnumerable<AttendanceSummaryDto>>> GetAttendanceSummaryAsync()
    {
        var records = await unitOfWork.AttendanceRecords.GetSummaryDataAsync();

        if (records == null || !records.Any())
            return _responseHandler.NotFound<IEnumerable<AttendanceSummaryDto>>("No attendance data available for summary.");

        var summary = records
            .GroupBy(a => new { a.StudentID, StudentName = a.Student != null ? a.Student.FullName : "Unknown" })
            .Select(g =>
            {
                int total = g.Count();
                int present = g.Count(a => a.Status == AttendanceStatus.Present);
                int absent = g.Count(a => a.Status == AttendanceStatus.Absent);
                int late = g.Count(a => a.Status == AttendanceStatus.Late);

                return new AttendanceSummaryDto
                {
                    StudentID = g.Key.StudentID,
                    StudentName = g.Key.StudentName,
                    TotalDays = total,
                    PresentDays = present,
                    AbsentDays = absent,
                    LateDays = late,
                    AbsencePercentage = total > 0 ? Math.Round((double)absent / total * 100, 1) : 0,
                    AttendancePercentage = total > 0 ? Math.Round((double)present / total * 100, 1) : 0
                };
            });

        return _responseHandler.Success(summary);
    }

    // ============================================================
    // PRIVATE MAPPER
    // ============================================================
    private static AttendanceRecordDto MapToDto(StudentBehaviorPlatform.Data.Entities.AttendanceRecord a)
    {
        return new AttendanceRecordDto
        {
            AttendanceID = a.AttendanceID,
            StudentID = a.StudentID,
            StudentName = a.Student != null ? a.Student.FullName : "Unknown",
            VideoSessionID = a.VideoSessionID,
            AttendanceDate = a.AttendanceDate,
            CheckInTime = a.CheckInTime,
            CheckOutTime = a.CheckOutTime,
            Status = a.Status,
            LateMinutes = a.LateMinutes,
            ConfidenceScore = a.ConfidenceScore,
            Source = a.Source
        };
    }





}