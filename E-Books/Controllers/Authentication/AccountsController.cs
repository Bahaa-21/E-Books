using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.Helper;
using E_Books.ViewModel.FromView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers.Authentication;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    public AccountsController(IAuthService authService, IEmailService emailService)
    => (_authService, _emailService) = (authService, emailService);


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Masseage);


        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(new
        {
            token = result.Token,
            refreshToken = result.RefreshToken,
            role = result.Roles,
            status = StatusCodes.Status200OK
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

        return Ok(new
        {
            token = result.Token,
            refreshToken = result.RefreshToken,
            FirstName = result.FirstName,
            LastName = result.LastName,
            role = result.Roles,
            status = StatusCodes.Status200OK
        });
    }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        var result = await _authService.ConfirmEmailAsync(userId, code);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok("Thank you for confirming your email");
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


    [HttpPost("revokeToken")]
    public async Task<IActionResult> RevokeToken([FromQuery] RevokeTokenVM revokeToken)
    {
        var token = revokeToken.Token ?? Request.Cookies["refreshToken"];

        if (token is null)
            return BadRequest("Token is required!");

        var result = await _authService.RevokeTokenAsync(token);
        if (!result)
            return BadRequest("Token is invalid!");

        return Ok();
    }



    [HttpPost("sendEmail")]
    public IActionResult SendEmai([FromBody] EmailDto requset)
    {
        _emailService.SendEmail(requset);
        return Ok();
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
