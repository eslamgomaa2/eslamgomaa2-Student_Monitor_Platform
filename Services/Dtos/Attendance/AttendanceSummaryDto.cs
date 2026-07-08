using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Attendance
{
    public class AttendanceSummaryDto
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LateDays { get; set; }
        public double AbsencePercentage { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
