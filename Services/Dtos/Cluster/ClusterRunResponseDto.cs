using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Cluster
{
    public class ClusterRunResponseDto
    {
        public int RunID { get; set; }
        public DateTime RunAt { get; set; }
        public string FiltersApplied { get; set; }
        public int NumClusters { get; set; }
        public List<ClusterSummaryDto> Clusters { get; set; }
        public List<ClusterVisualizationPointDto> VisualizationData { get; set; }
    }
}
