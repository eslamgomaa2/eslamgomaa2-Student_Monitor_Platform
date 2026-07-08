using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class IncidentDto
    {
        public int IncidentID { get; set; }
        public string Description { get; set; }
        public DateTime OccurredAt { get; set; }
        public string Type { get; set; }
    }
}
