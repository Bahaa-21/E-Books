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
            FisrtName = result.FirstName,
            LastName = result.LastName,
            role = result.Roles,
            status = StatusCodes.Status202Accepted
        });
    }

    [Authorize(Roles = "Adminsitrator")]
    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddRoleAsync(model);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(result);
    }




    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefershToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _authService.RefreshTokenAsync(refreshToken);

        if (!result.IsAuthenticated)
            return BadRequest(result);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
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
