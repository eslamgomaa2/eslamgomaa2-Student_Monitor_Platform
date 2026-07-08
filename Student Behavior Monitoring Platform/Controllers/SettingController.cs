using E_Learning.API.Extensions.E_Learning.API.Extensions;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles ="Teacher")]
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

        [HttpGet("admin-profile")]
        public async Task<IActionResult> GetAdminProfile()
        {
            var userId = User.GetUserId();
            var result = await _adminProfileServices.GetAdminProfile(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpPut("admin-profile")]
        public async Task<IActionResult> UpdateAdminInformation([FromBody] ProfileInformationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetUserId();
            var result = await _adminProfileServices.UpdateAdminInformationAsync(userId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
           
            [FromBody] ChangePasswordDto dto)
        {
            var userId = User.GetUserId();
            var result = await _genericSetting.UpdatePasswordAsync(userId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpPost("profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture( [FromForm]UpdateAdminProfilePicture dto)
        {
            var userId = User.GetUserId();
            var result = await _genericSetting.UploadProfilePictureAsync(userId, dto.Picture!);
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpDelete("profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var userId = User.GetUserId();
            var result = await _genericSetting.DeleteProfilePictureAsync(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.GetUserId();
            var result = await _genericSetting.DeleteAccount(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }


    }
}