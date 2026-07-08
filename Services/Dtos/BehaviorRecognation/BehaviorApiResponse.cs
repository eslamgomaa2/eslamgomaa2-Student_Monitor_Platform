using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRecognation
{
    public class BehaviorApiResponse
    {
        public bool Success { get; set; }
        public int StudentsFound { get; set; }
        public List<BehaviorDetectionDto> Detections { get; set; }
    }
}
