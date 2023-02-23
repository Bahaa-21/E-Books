using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminProfileController : ControllerBase
{
    private readonly UserManager<UsersApp> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _service;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    private readonly IMapper _mapper;
    public AdminProfileController(UserManager<UsersApp> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork service, IAuthService authService ,IUserService userService, IMapper mapper) =>
    (_userManager, _roleManager, _service, _authService ,_userService , _mapper) = (userManager, roleManager, service, authService , userService, mapper);



    [HttpGet("get-profile-admin")]
    public async Task<IActionResult> GetProfile()
    {
        var adminProfile = await _userService.GetUserProfile();

        if (adminProfile is null)
            return Unauthorized($"Unauthorized ,{ModelState}");
        
        var response = _mapper.Map<UserProfileVM>(adminProfile);

        return Ok(response);
    }


    [HttpPost("upload-image")]

    public async Task<IActionResult> UploadImage([FromBody] PhotoVM photoVM)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");


        var user = await _userService.GetUserProfile();

        if (user is null)
            return NotFound($"This user not exiset");


        var photo = new Photo()
        {
            Image = photoVM.ProfilePhoto,
            UserId = user.Id
        };

        await _service.Photo.AddAsync(photo);
        await _service.SaveAsync();

        return Created(nameof(UploadImage), new { photo.Id, photo.Image });
    }


    [HttpDelete("remove-image")]
    public async Task<IActionResult> RemoveImageAsync()
    {

       var user = await _userService.GetUserProfile();

        if (user is null)
            return BadRequest();

        var img = await _service.Photo.GetAsync(ph => ph.UserId == user.Id , null);

        _service.Photo.Delete(img);

        await _service.SaveAsync();
        return NoContent();
    }




    [HttpPut("update-profile")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileVM adminProfileVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.GetUserProfile();

        if (user is null)
            return Unauthorized();

        _mapper.Map(adminProfileVM, user);

        var result = await _userService.UpdateProfile(user);

        var response = _mapper.Map<UserProfileVM>(user);
        if (!result)
            return BadRequest(user);

        return Ok(response);
    }
}