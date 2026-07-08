using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Student
{
    public class AddStudentDto
    {
        public string FullName { get; set; } = string.Empty;
        public string NationalID { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; } = string.Empty;
        public string? GradeLevel { get; set; } = string.Empty;
        public string? Section { get; set; } = string.Empty;
        public int AcademicYear { get; set; }

        // الـ AI محتاج StudentCode عشان الـ Mapping الداخلي
        public string StudentCode { get; set; } = string.Empty;

        // صورة الطالب للـ Face Registration
        public IFormFile ImageFile { get; set; } = null!;
    }
}
