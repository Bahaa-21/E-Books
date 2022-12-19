using E_Books.IServices;
using E_Books.ViewModel;

namespace E_Books.Services;

public class AuthService : IAuthService
{
    public Task<string> AddRoleAsync(AddRoleModel model)
    {
        throw new NotImplementedException();
    }

    public Task<AuthModel> GetTokenAsync(TokenRequestModel model)
    {
        throw new NotImplementedException();
    }

    public Task<AuthModel> RefreshTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        throw new NotImplementedException();
    }
}
