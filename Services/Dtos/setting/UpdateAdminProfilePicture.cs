using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.setting
{
    public class UpdateAdminProfilePicture
    {
        public IFormFile? Picture { get; set; }
    }
}
