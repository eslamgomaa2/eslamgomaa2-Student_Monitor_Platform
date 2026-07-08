using E_Learning.Core.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ResponseHandler _responseHandler;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager, ResponseHandler responseHandler, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _responseHandler = responseHandler;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return _responseHandler.NotFound<AuthResponse>("Invalid credentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return _responseHandler.BadRequest<AuthResponse>("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwt(user, roles.ToList());

            var userprofile = await _unitOfWork.AdminProfiles.GetByIdAsync(user.Id);

            return _responseHandler.Success(new AuthResponse
            {
                Id = user.Id,
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                Roles = roles.ToList(),
                Token = token,
                ProfilePhotoPath =userprofile?.ProfilePicture
            });
        }

        public async Task<Response<bool>> LogoutAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return _responseHandler.NotFound<bool>("User not found.");

            var result = await _userManager.UpdateSecurityStampAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<bool>($"Logout failed: {errors}");
            }

            

            return _responseHandler.Success(true);
        }

        public async Task<Response<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return _responseHandler.BadRequest<AuthResponse>("Email is already registered.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<AuthResponse>($"Registration failed: {errors}");
            }

            if (!await _userManager.IsInRoleAsync(user, request.Role))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                    return _responseHandler.BadRequest<AuthResponse>($"Role assignment failed: {errors}");
                }
            }

                if (request.Role == "Teacher")
            {
                var profile = new StudentBehaviorPlatform.Data.Entities.AdminProfile
                {
                    AppUserId = user.Id
                };
                await _unitOfWork.AdminProfiles.AddAsync(profile);
                await _unitOfWork.SaveChangesAsync();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwt(user, roles.ToList());

            var response = new AuthResponse
            {
                Id = user.Id,
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                Roles = roles.ToList(),
                Token = token

            };

            return _responseHandler.Created(response);
        }

        private string GenerateJwt(ApplicationUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.FullName ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]
                           ?? throw new InvalidOperationException("Jwt:SecretKey is not configured."));

            if (keyBytes.Length < 32)
                throw new InvalidOperationException("Jwt:SecretKey must be at least 256 bits (32 characters).");

            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:AccessTokenExpiryMinutes")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
