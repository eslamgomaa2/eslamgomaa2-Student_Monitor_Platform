using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class StudentDetailDto
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; } // e.g., "S-1005"
        public string GradeLevel { get; set; } // e.g., "GRADE 12"
        public double CurrentGrade { get; set; }
        public double AttendanceRate { get; set; }
        public int ClusterGroupID { get; set; }
        public string ClusterLabel { get; set; }
        public List<IncidentDto> RecentIncidents { get; set; }
    }
}
