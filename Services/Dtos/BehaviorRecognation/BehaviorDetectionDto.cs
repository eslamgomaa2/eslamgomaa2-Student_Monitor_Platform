using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRecognation
{
    public class BehaviorDetectionDto
    {
        public List<int> Bbox { get; set; }
        public string Behavior { get; set; }
        public decimal Confidence { get; set; }
    }
}
