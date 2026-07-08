

using E_Learning.Core.Base;
using Services.Dtos.Attendance;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentMonitor.Core.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceRecord> MarkAttendanceAsync(Stream cameraImage, DateTime scheduledTime, int? videoSessionId = null, CancellationToken ct = default);
    Task<Response<IEnumerable<AttendanceRecordDto>>> GetAllAttendanceAsync();
    Task<Response<IEnumerable<AttendanceRecordDto>>> GetAttendanceByStudentAsync(int studentId);
    Task<Response<IEnumerable<AttendanceRecordDto>>> GetAttendanceByDateAsync(DateTime date);
    Task<Response<IEnumerable<AttendanceSummaryDto>>> GetAttendanceSummaryAsync();



}