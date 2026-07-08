using E_Learning.Core.Base;
using Services.Dtos.setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.AdminProfile
{
    public interface IAdminProfileServices
    {

        Task<Response<AdminUpdateinformationResponseDto>> UpdateAdminInformationAsync(int userId, ProfileInformationDto dto);
        Task<Response<AdminResponseDto>> GetAdminProfile(int userId);

        
    }
}
