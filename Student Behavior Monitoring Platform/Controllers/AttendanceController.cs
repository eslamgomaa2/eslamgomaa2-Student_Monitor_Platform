using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Attendance;
using Services.Services.FaceRecognition;
using StudentMonitor.Core.Interfaces;

namespace StudentMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly IFaceRecognitionService _faceRecognition;

    public AttendanceController(
        IAttendanceService attendanceService,
        IFaceRecognitionService faceRecognition)
    {
        _attendanceService = attendanceService;
        _faceRecognition = faceRecognition;

    }

    /// <summary>
    /// 📸 Endpoint الرئيسي - الكاميرا تبعت صورة وهنا نسجل الحضور
    [HttpPost("mark")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> MarkAttendance([FromForm] CameraCaptureDto cameraCaptureDto)
    {
        try
        {
            if (cameraCaptureDto.CameraImage == null || cameraCaptureDto.CameraImage.Length == 0)
                return BadRequest(new { error = "❌ No image provided!" });

            await using var stream = cameraCaptureDto.CameraImage.OpenReadStream();

            var record = await _attendanceService.MarkAttendanceAsync(stream, cameraCaptureDto.ScheduledTime, cameraCaptureDto.VideoSessionId);

            return Ok(new
            {
                success = true,
                message = $" {record.Status} recorded!",
                data = new
                {
                    attendanceId = record.AttendanceID,
                    studentId = record.StudentID,
                    studentName = record.Student?.FullName,
                    attendanceDate = record.AttendanceDate,
                    checkInTime = record.CheckInTime,
                    status = record.Status.ToString(),
                    lateMinutes = record.LateMinutes,
                    confidence = record.ConfidenceScore,
                    source = record.Source,
                    videoSessionId = record.VideoSessionID
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("register-student")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> RegisterStudent([FromForm] StudentFaceDto studentFaceDto)
    {
        try
        {
            if (studentFaceDto.FaceImage == null || studentFaceDto.FaceImage.Length == 0)
                return BadRequest(new { error = "❌ No image provided!" });


            await using var stream = studentFaceDto.FaceImage.OpenReadStream();
            var success = await _faceRecognition.RegisterFaceAsync(
                studentFaceDto.StudentId, studentFaceDto.StudentCode, studentFaceDto.StudentName, stream);

            if (!success)
                return StatusCode(500, new { error = "❌ Failed to register face in AI model" });




            return Ok(new
            {
                success = true,
                message = $"✅ Student {studentFaceDto.StudentName} registered successfully!",
                studentId = studentFaceDto.StudentId,
                studentCode = studentFaceDto.StudentCode
            });
        }
        catch (Exception ex)
        {

            return StatusCode(500, new { error = ex.Message });
        }
    }

    
    [HttpGet("GetAllAttendance")]
    public async Task<IActionResult> GetAllAttendance()
    {
        var result = await _attendanceService.GetAllAttendanceAsync();
        return StatusCode((int)result.HttpStatusCode, result);
    }

    
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetAttendanceByStudent(int studentId)
    {
        var result = await _attendanceService.GetAttendanceByStudentAsync(studentId);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    
    [HttpGet("date/{date}")]
    public async Task<IActionResult> GetAttendanceByDate(DateTime date)
    {
        var result = await _attendanceService.GetAttendanceByDateAsync(date);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    
    [HttpGet("summary")]
    public async Task<IActionResult> GetAttendanceSummary()
    {
        var result = await _attendanceService.GetAttendanceSummaryAsync();
        return StatusCode((int)result.HttpStatusCode, result);
    }
}