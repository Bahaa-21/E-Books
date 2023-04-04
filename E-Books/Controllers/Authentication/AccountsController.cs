using E_Books.BusinessLogicLayer.Abstract;
using E_Books.ViewModel.FromView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers.Authentication;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountsController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Masseage);

        return Accepted(new
        {
            token = result.Token,
            role = result.Roles,
            status = StatusCodes.Status202Accepted
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.GetTokenAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Masseage);

        if (!string.IsNullOrEmpty(result.RefreshToken))
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Accepted(new
        {
            token = result.Token,
            FirstName = result.FirstName,
            LastName = result.LastName,
            role = result.Roles,
            status = StatusCodes.Status202Accepted
        });
    }

    
    // Get api/Accounts/SigningGoolge/signing-google/5
    [HttpGet("signing-google/{id}")]
    [Authorize]
    public  IActionResult SigningGoolge(int id)
    {
        var user =  this.User.Identity.Name;
        return Ok(user);
    }


    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefershToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _authService.RefreshTokenAsync(refreshToken);

        if (!result.IsAuthenticated)
            return BadRequest(result);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(new
        {
            token = result.Token,
            UserName = result.UserName,
            role = result.Roles,
        });
    }


    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOption = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime()
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOption);
    }
}
