using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.setting
{
    public class AdminResponseDto
    {
        public string  Fullname { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Email { get; set; }
        public bool EmailNotificationsEnabled { get; set; }

        public bool PushNotificationsEnabled { get; set; }

        public string? Role { get; set; }
        public string Language { get; set; }
    }
}
