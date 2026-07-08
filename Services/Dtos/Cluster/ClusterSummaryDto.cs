using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class ClusterSummaryDto
    {
        public int GroupID { get; set; }
        public string ClusterLabel { get; set; } // e.g., "At-Risk", "Disengaged", "High Potential"
        public string ClusterName { get; set; }  // e.g., "CLUSTER A"
        public int StudentCount { get; set; }
        public double AvgAttendance { get; set; }
        public decimal AvgGrade { get; set; }
        public string MainIssue { get; set; }
        public string ColorCode { get; set; } // For UI styling
    }
}
