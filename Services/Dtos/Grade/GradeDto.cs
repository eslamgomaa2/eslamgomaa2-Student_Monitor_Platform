using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Grade
{
    public class GradeDto
    {
        public int GradeID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string? Subject { get; set; }
        public decimal Score { get; set; }
        public string? GradeLabel { get; set; }
        public string? Term { get; set; }
        public int AcademicYear { get; set; }
        public DateTime Date { get; set; }
    }
}
