using Core.Enums;
using System;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class AttendanceRecord
    {
        public int AttendanceID { get; set; }
        public int StudentID { get; set; }
        public int? VideoSessionID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public int? LateMinutes { get; set; }
        public double? ConfidenceScore { get; set; }
        public string? Source { get; set; }

        // Navigation
        public Student? Student { get; set; }
        public VideoSession? VideoSession { get; set; }
    }
}