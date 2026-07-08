using E_Learning.Core.Base;

public interface IAuthService
{
    Task<Response<AuthResponse>> LoginAsync(LoginRequest request);
    Task<Response<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Response<bool>> LogoutAsync(int userId);
  //  Task<Response<AuthResponse>> GetCurrentUserSessionAsync(int userId);

}