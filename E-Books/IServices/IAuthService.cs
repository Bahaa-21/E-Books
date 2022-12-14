using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;

namespace E_Books.IServices;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterModel model);
    Task<AuthModel> GetTokenAsync(TokenRequestModel model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthModel> RefreshTokenAsync(string token);
    // Task<bool> RevokeTokenAsync(string token);
}
