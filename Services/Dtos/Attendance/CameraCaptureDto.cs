using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Attendance
{
    public class CameraCaptureDto
    {
        public IFormFile CameraImage { get; set; }

        public DateTime ScheduledTime { get; set; }

        public int? VideoSessionId { get; set; }
    }
}
