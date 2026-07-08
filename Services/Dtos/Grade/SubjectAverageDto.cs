using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Grade
{
    public class SubjectAverageDto
    {
        public string Subject { get; set; }
        public double AverageScore { get; set; }
        public int TotalStudents { get; set; }
        public decimal HighestScore { get; set; }
        public decimal LowestScore { get; set; }
    }
}
