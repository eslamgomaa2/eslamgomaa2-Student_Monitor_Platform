using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class GenerateClusterReportDto
    {
        public string DateRange { get; set; }
        public string SchoolYear { get; set; }
        public string GradeLevel { get; set; }
       
    }
}
