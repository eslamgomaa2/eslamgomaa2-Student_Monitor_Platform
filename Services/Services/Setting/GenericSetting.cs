using E_Learning.Core.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Dtos.setting;
using Services.Services.FileStorge;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Setting
{
    public class GenericSetting: IGenericSetting
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ResponseHandler _responseHandler;
        private readonly IFileStorge _fileStorage;

        public GenericSetting(IFileStorge fileStorage, ResponseHandler responseHandler, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _fileStorage = fileStorage;
            _responseHandler = responseHandler;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> UpdatePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());


            if (user is null)
                return _responseHandler.NotFound<string>("User not found");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
            if (!passwordValid)
                return _responseHandler.BadRequest<string>("Current password is incorrect");
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return _responseHandler.BadRequest<string>("New password and confirmation do not match");
            }
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<string>($"Failed to change password: {errors}");
            }

            return _responseHandler.Success("Password changed successfully");
        }


        public async Task<Response<string>> UploadProfilePictureAsync(int userId, IFormFile file)
        {
            var profile = await _unitOfWork.AdminProfiles.GetProfileByUserIdAsync(userId);

            if (profile is null)
                return _responseHandler.NotFound<string>("Profile not found");


            if (!string.IsNullOrEmpty(profile.ProfilePicture))
                await _fileStorage.DeleteFileAsync(profile.ProfilePicture);

            var relativePath = await _fileStorage.SaveFileAsync(file, "profiles");
            var publicUrl = _fileStorage.GetPublicUrl(relativePath);

            profile.ProfilePicture = relativePath;
            _unitOfWork.AdminProfiles.Update(profile);
            await _unitOfWork.SaveChangesAsync();
            return _responseHandler.Success(publicUrl);
        }


        public async Task<Response<string>> DeleteProfilePictureAsync(int userId)
        {
            var profile = await _unitOfWork.AdminProfiles.GetProfileByUserIdAsync(userId);

            if (profile is null)
                return _responseHandler.NotFound<string>("Profile not found");


            if (string.IsNullOrEmpty(profile.ProfilePicture))
                return _responseHandler.BadRequest<string>("No profile picture to remove");

            await _fileStorage.DeleteFileAsync(profile.ProfilePicture);

            profile.ProfilePicture = null;
            _unitOfWork.AdminProfiles.Update(profile);
            await _unitOfWork.SaveChangesAsync();


            return _responseHandler.Success("Profile picture removed successfully");
     
        
        }


        public async Task<Response<string>> DeleteAccount(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return _responseHandler.NotFound<string>("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<string>($"Failed to delete account: {errors}");
            }

            var userProfile = await _unitOfWork.AdminProfiles.GetProfileByUserIdAsync(id);

            if (userProfile != null)
            {
                _unitOfWork.AdminProfiles.Remove(userProfile);
            }

            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success("Account deleted successfully");
        }
    }

}
