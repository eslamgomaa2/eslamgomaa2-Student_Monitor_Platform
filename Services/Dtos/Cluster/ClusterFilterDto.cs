using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class ClusterFilterDto
    {
        public string DateRange { get; set; } = "Last 90 days";
        public string SchoolYear { get; set; } = "2025 - 2026";
        public string GradeLevel { get; set; } = "All grades";
    }
}
