using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.setting
{
    public class ProfileInformationDto
    {
        public string? FullName { get; set; }
       public langauage  Language { get; set; } = langauage.English;
        public bool EmailNotificationsEnabled { get; set; } = true;
        public bool PushNotificationsEnabled { get; set; } = true;
    }
}
