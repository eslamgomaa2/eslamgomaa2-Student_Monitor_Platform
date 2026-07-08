using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class ClusterVisualizationPointDto
    {
        public int StudentID { get; set; }
        public string StudentCode { get; set; }      // ← S-1001, S-1002, etc.
        public string StudentName { get; set; }
        public string GradeLevel { get; set; }        // ← Grade 9, Grade 10, etc.
        public double AverageGrade { get; set; }
        public double AttendanceRate { get; set; }    // ← بدل AbsenteeismRate
        public int GroupID { get; set; }
        public string GroupLabel { get; set; }        // ← Cluster A, Cluster B
        public string RiskLabel { get; set; }         // ← At-Risk, Disengaged, High Potential
        public string ColorCode { get; set; }
    }
}
