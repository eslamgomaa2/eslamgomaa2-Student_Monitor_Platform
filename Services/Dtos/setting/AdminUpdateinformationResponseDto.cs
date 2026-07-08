using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.setting
{
    public class AdminUpdateinformationResponseDto
    {
        

            public string? Fullname { get; set; }
            public bool EmailNotificationsEnabled { get; set; }

            public bool PushNotificationsEnabled { get; set; }

            public string Language { get; set; }
        
    }

}
