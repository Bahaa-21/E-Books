using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Books.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.DataAccessLayer;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http.Headers;
using RestSharp;

namespace E_Books.BusinessLogicLayer.Concrete;

public class AuthService : IAuthService
{
    public readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<UsersApp> _userManager;
    private readonly IHttpContextAccessor _httpContext;
    private readonly JWT _jwt;
    private readonly EmailConfirm _emailConfirm;

    public AuthService(IHttpContextAccessor httpContext, UserManager<UsersApp> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IOptions<EmailConfirm> emailConfirm)
    {
        (_httpContext, _userManager, _roleManager, _jwt, _context, _emailConfirm) = (httpContext, userManager, roleManager, jwt.Value, context, emailConfirm.Value);
    }

    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Masseage = "Email is already registered!" };

        var user = new UsersApp()
        {
            Email = model.Email,
            UserName = string.Concat(model.FirstName, model.LastName.Substring(0, 4)),
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Address = "Damascus,Syria",
            Gender = model.Gender
        };

        var resutl = await _userManager.CreateAsync(user, model.Password);

        if (!resutl.Succeeded)
        {
            string errors = string.Empty;
            foreach (var error in resutl.Errors)
                errors += $"{error.Description},";

            return new AuthModel { Masseage = errors };
        }

        await _userManager.AddToRoleAsync(user, "User");

        var jwtToken = await CreateJwtToken(user);

        var refreshToken = GenerateRefreshToken();
        user.RefreshTokens?.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        // var _code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        // var callBackUrl = urlHelper.Action("ConfirmEmail",
        //                                     "Accounts",
        //                                     new { userId = user.Id, code = _code },
        //                                     protocol: controller.HttpContext.Request.Scheme);

        // string emailBody = $"Confirm your registration via this link: <a href=''>link</a>";

        // SendEmailAsync(emailBody , _code);

        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = jwtToken.ValidTo,
            IsAuthenticated = true,
            Roles = new List<string> { "User" },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            FirstName = user.FirstName,
            LastName = user.LastName,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiration = refreshToken.ExpiresOn
        };
    }

    public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
    {
        var authModel = new AuthModel();
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authModel.Masseage = "Email or Password is incorrect";
            return authModel;
        }

        var jwtToken = await CreateJwtToken(user);
        var roleList = await _userManager.GetRolesAsync(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authModel.Email = user.Email;
        authModel.FirstName = user.FirstName;
        authModel.LastName = user.LastName;
        authModel.Roles = roleList.ToList();

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authModel.RefreshToken = activeRefreshToken.Token;
            authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
        }

        return authModel;
    }

    public async Task<string> ConfirmEmailAsync(string userId, string code)
    {

        if (userId is null || code is null)
            return "Invalid email confirmation url";

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return "Invalid email parameter";


        // code = Encoding.UTF8.GetString(Convert.FromBase64String(code));

        var result = await _userManager.ConfirmEmailAsync(user, code);

        var status = result.Succeeded ? string.Empty : "Your email is not confirmed , Please try again later";

        return status;
    }

    public async Task<UsersApp> CreateAdminAccountAsync(CreateAdminVM model)
    {
       

        UsersApp user = new()
        {
            UserName = string.Concat(model.FirstName, model.LastName.Substring(0, 5)),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Gender = model.Gender,
        };

        var createAccount = await _userManager.CreateAsync(user, model.Password);
        var addToRole = await _userManager.AddToRoleAsync(user, model.Role);

        return user;
    }
    public async Task<string> CreateAdminAccountValidate(string role, string email)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            return "role is invalid";

        if (!email.Contains("@safa7at"))
            return $"The Email ({email}) is wrong , must contain @safa7at";
        return string.Empty;    
    }

    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        var authModel = new AuthModel();

        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
        {
            authModel.Masseage = "Invalid token";
            return authModel;
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
        {
            authModel.Masseage = "Inactive token";
            return authModel;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        //Create and return JwtToken
        var jwtToken = await CreateJwtToken(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var roles = await _userManager.GetRolesAsync(user);
        authModel.Email = user.Email;
        authModel.UserName = user.UserName;
        authModel.Roles = roles.ToList();
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.ExpiresOn = jwtToken.ValidTo;
        authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return authModel;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
            return false;

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
            return false;

        refreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return true;
    }

    public async Task<string> DeleteAdminAsync(string adminId)
    {
        var admin = await _userManager.FindByIdAsync(adminId);
        if (admin is null)
            return "This role not existe";
        var result = await _userManager.DeleteAsync(admin);
        return result.Succeeded ? string.Empty : "Something went wrong";
    }


    public async Task<string> UpdateRoleAsync(IdentityRole role)
    {
        var result = await _roleManager.UpdateAsync(role);
        return result.Succeeded ? string.Empty : "Somthing went wrong";
    }


    public async Task<List<IdentityRole>> GetAllRolesAsync() => await _roleManager.Roles.ToListAsync();

    public async Task<IdentityRole> GetRoleAsync(string roleId) =>
        await _roleManager.FindByIdAsync(roleId);

    public async Task<IList<UsersApp>> GetAllAdmins() =>
    await _userManager.GetUsersInRoleAsync("Admin");



    #region Create JWT Token
    private async Task<JwtSecurityToken> CreateJwtToken(UsersApp user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name , user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email , user.Email),
        }
        .Union(roleClaims)
        .Union(userClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurrityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audinece,
            expires: DateTime.UtcNow.AddHours(_jwt.DurationInHours),
            claims: claim,
            signingCredentials: signingCredentials
            );

        return jwtSecurrityToken;
    }
    #endregion


    #region Generate Refresh Token
    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = new RNGCryptoServiceProvider();
        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(5),
            CreatedOn = DateTime.UtcNow
        };

    }
    #endregion


    #region Send Email
    private void SendEmailAsync(string body, string email)
    {
        var authToken = Encoding.ASCII.GetBytes($"api:{_emailConfirm.ApiKey}");

        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

        var client = new RestClient(httpClient);

        var request = new RestRequest("", Method.Post);
        request.AddParameter("domain", "sandbox50cdb4c47e5347cf84751316e18127ef.mailgun.org");
        request.Resource = "{domain}/messages";
        request.AddParameter("from", "Admin <postmaster@sandbox50cdb4c47e5347cf84751316e18127ef.mailgun.org>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Email Verification");
        request.AddParameter("text", body);
        request.Method = Method.Post;

        client.Execute(request);
    }
    #endregion
}
