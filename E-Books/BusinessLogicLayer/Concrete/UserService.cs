using System.Security.Claims;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.Data;
using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Books.BusinessLogicLayer.Concrete;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UsersApp> _userManager;
    private readonly IHttpContextAccessor _httpContext;

    public UserService(UserManager<UsersApp> userManager, ApplicationDbContext context, IHttpContextAccessor httpContext) => (_context, _userManager, _httpContext) = (context, userManager, httpContext);


    public async Task<UsersApp> GetUserProfile()
    {
        string email = string.Empty;

        if (_httpContext.HttpContext != null)
            email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);

        if (!string.IsNullOrEmpty(email))
        {
            var userProfile = await _userManager.Users.Include(ph => ph.Photos).SingleOrDefaultAsync(userEmail => userEmail.Email == email);
            return userProfile;
        }
        return null;
    }

    
    public async Task<bool> UpdateProfile(UsersApp usersApp)
    {
        if (usersApp is null)
            return false;

        var result = await _userManager.UpdateAsync(usersApp);
        if (!result.Succeeded)
        {
            string errors = string.Empty;
            foreach (var error in result.Errors)
                errors += $"{error.Description} ,";

            return false;
        }
        return true;
    }
}
