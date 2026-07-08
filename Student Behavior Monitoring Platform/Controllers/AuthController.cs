using E_Learning.API.Extensions.E_Learning.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentBehaviorPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPost("register")]
       
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var result = await _authService.RegisterAsync(request);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userId = User.GetUserId();
            var result = await _authService.LogoutAsync(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

       /* [HttpGet("session")]
        [Authorize]
        public async Task<IActionResult> GetCurrentSession(CancellationToken ct)
        {
            var userId = User.GetUserId();
            var result = await _authService.GetCurrentUserSessionAsync(userId);
            return StatusCode((int)result.HttpStatusCode, result);
        }*/
    }
}