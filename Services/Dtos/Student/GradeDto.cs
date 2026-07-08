using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Student
{
    public record GradeDto(
         int GradeID,
    int StudentID,
    string? Subject,
    decimal Score,
    string? GradeLabel,
    string? Term,
    int AcademicYear
     );
}
