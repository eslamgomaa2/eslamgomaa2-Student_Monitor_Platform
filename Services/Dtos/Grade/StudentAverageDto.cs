using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Grade
{
    public class StudentAverageDto
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public double AverageScore { get; set; }
        public int TotalSubjects { get; set; }
        public decimal HighestScore { get; set; }
        public decimal LowestScore { get; set; }
    }
}
