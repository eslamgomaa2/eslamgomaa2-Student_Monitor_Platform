using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Attendance
{
    public class StudentFaceDto
    {
        public int StudentId { get; set; }

        public string StudentCode { get; set; }

        public string StudentName { get; set; }

        public IFormFile FaceImage { get; set; }
    }
}
