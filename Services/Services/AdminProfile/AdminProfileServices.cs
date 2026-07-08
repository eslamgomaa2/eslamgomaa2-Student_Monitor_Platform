

using E_Learning.Core.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Dtos.setting;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;

namespace Services.Services.AdminProfile
{
    public class AdminProfileServices : IAdminProfileServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ResponseHandler _responseHandler;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminProfileServices(IUnitOfWork unitOfWork, ResponseHandler responseHandler, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _userManager = userManager;
        }

        public async Task<Response<AdminResponseDto>> GetAdminProfile(int userId)
        {
            var user = await unitOfWork.AdminProfiles.GetProfileByUserIdAsync(userId);

            if (user == null || user.AppUser == null)
                return _responseHandler.NotFound<AdminResponseDto>("User not found");

            var roles = await _userManager.GetRolesAsync(user.AppUser);

            return _responseHandler.Success(new AdminResponseDto
            {
                Fullname = user.AppUser.FullName,
                Email = user.AppUser.Email,
                EmailNotificationsEnabled = user.EmailNotificationsEnabled,
                PushNotificationsEnabled = user.PushNotificationsEnabled,
                Role = roles.FirstOrDefault(), // safe handling
                Language = user.Language,
                ProfilePicture = user.ProfilePicture
            });
        }

        public async Task<Response<AdminUpdateinformationResponseDto>> UpdateAdminInformationAsync(int userId, ProfileInformationDto dto)
        {
           
            var user = await  unitOfWork.AdminProfiles.GetProfileByUserIdAsync(userId);
            
           
            if (user == null)
            {
                return _responseHandler.NotFound<AdminUpdateinformationResponseDto>("User not found");
            }
            user.EmailNotificationsEnabled = dto.EmailNotificationsEnabled;
            user.PushNotificationsEnabled = dto.PushNotificationsEnabled;
            user.Language = dto.Language.ToString();
            user.AppUser.FullName = dto.FullName;

          
            await unitOfWork.SaveChangesAsync();
            return _responseHandler.Success<AdminUpdateinformationResponseDto>(new AdminUpdateinformationResponseDto
            {
                EmailNotificationsEnabled = user.EmailNotificationsEnabled,
                PushNotificationsEnabled = user.PushNotificationsEnabled,
                Language = user.Language,
                Fullname = user.AppUser.FullName
            });



        }
    }
}
