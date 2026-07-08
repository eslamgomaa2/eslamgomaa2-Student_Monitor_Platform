using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class GenerateClusterReportResponseDto
    {
        public int RunID { get; set; }
        public string ReportPath { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string Message { get; set; }
    }
}
