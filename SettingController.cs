using Microsoft.AspNetCore.Mvc;
using Services.Dtos.setting;
using Services.Services.AdminProfile;
using Services.Services.Setting;
using System.Threading;
using System.Threading.Tasks;

namespace StudentBehaviorPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly IAdminProfileServices _adminProfileServices;
        private readonly IGenericSetting _genericSetting;

        public SettingController(
            IAdminProfileServices adminProfileServices,
            IGenericSetting genericSetting)
        {
            _adminProfileServices = adminProfileServices;
            _genericSetting = genericSetting;
        }

        [HttpGet("admin-profile/{userId}")]
        public async Task<IActionResult> GetAdminProfile(int userId)
        {
            var result = await _adminProfileServices.GetAdminProfile(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpPut("admin-profile/{userId}")]
        public async Task<IActionResult> UpdateAdminInformation(
            int userId,
            [FromBody] ProfileInformationDto dto)
        {
            var result = await _adminProfileServices.UpdateAdminInformationAsync(userId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(
            int userId,
            [FromBody] ChangePasswordDto dto)
        {
            var result = await _genericSetting.UpdatePasswordAsync(userId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

       
        [HttpPost("profile-picture/{userId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture(int userId, [FromForm] Microsoft.AspNetCore.Http.IFormFile file)
        {
            var result = await _genericSetting.UploadProfilePictureAsync(userId, file);
            return StatusCode((int)result.HttpStatusCode, result);
        }

       
        [HttpDelete("profile-picture/{userId}")]
        public async Task<IActionResult> DeleteProfilePicture(int userId)
        {
            var result = await _genericSetting.DeleteProfilePictureAsync(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("account/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _genericSetting.DeleteAccount(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }


    }
}