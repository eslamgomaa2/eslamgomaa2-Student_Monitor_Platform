using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Attendance
{
    public class AttendanceRecordDto
    {
        public int AttendanceID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int? VideoSessionID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public string StatusDisplay => Status.ToString();
        public int? LateMinutes { get; set; }
        public double? ConfidenceScore { get; set; }
        public string? Source { get; set; }
    }
}
