using E_Learning.Core.Base;
using Microsoft.AspNetCore.Http;
using Services.Dtos.setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Setting
{
    public interface IGenericSetting
    {
        Task<Response<string>> UpdatePasswordAsync(int userId, ChangePasswordDto dto);
        Task<Response<string>> UploadProfilePictureAsync(int userId, IFormFile file);
        Task<Response<string>> DeleteProfilePictureAsync(int userId);
        Task<Response<string>> DeleteAccount(int id);
    }
}
