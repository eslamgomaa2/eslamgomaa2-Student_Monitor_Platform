using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRecognation
{
    public class CameraBehaviorDto
    {
        public IFormFile CameraImage { get; set; }
       
    }
}
