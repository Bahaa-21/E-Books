using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Books.Settings;
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

namespace E_Books.BusinessLogicLayer.Concrete;

public class AuthService : IAuthService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<UsersApp> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContext;
    private readonly JWT _jwt;

    public AuthService(IHttpContextAccessor httpContext, UserManager<UsersApp> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
    {
        (_httpContext, _userManager, _roleManager, _jwt) = (httpContext, userManager, roleManager, jwt.Value);
    }

    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Masseage = "Email is already registered!" };

        var user = new UsersApp()
        {
            Email = model.Email,
            UserName = string.Concat(model.FirstName, model.LastName.Substring(0 , 4)),
            FirstName = model.FirstName,
            LastName = model.LastName,
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

    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            return "User or role is invalid";
        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User is already assigned to this role";

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        return result.Succeeded ? string.Empty : "Something went wrong";

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
            new Claim(JwtRegisteredClaimNames.Sub , user.Id),
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
}
