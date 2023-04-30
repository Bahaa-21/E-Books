using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly UserManager<UsersApp> _userManager;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public ProfileController(IUnitOfWork service,
                                 IMapper mapper,
                                 UserManager<UsersApp> userManager,
                                 IUserService userService
                                ) =>
    (_service, _mapper, _userManager, _userService) = (service, mapper, userManager, userService);




    [HttpGet("get-user-profile")]
    public async Task<IActionResult> GetUserProfle()
    {
        var userProfile = await _userService.GetUser();

        var response = _mapper.Map<UserProfileVM>(userProfile);

        return Ok(response);
    }




    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromBody] PhotoVM photoVM)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");


        var user = await _userService.GetUser();

        if (user.Photos is not null)
            _service.Photo.Delete(user.Photos);

        var photo = new Photo()
        {
            Image = photoVM.ProfilePhoto,
            UserId = user.Id
        };

        await _service.Photo.AddAsync(photo);
        await _service.SaveAsync();

        return Created(nameof(UploadImage), _mapper.Map<DisplayPhotoVM>(photo));
    }


    [HttpPatch("update-user-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.GetUser();

        if (user is null)
            return Unauthorized();

        _mapper.Map(model, user);

        _service.Users.Update(user);

        await _service.SaveAsync();

        var response = _mapper.Map<UpdateProfileVM>(user);

        return Ok(response);
    }



    [HttpDelete("remove-image")]
    public async Task<IActionResult> RemoveImageAsync()
    {

        var user = await _userService.GetUser();

        if (user.Photos is null)
            return NotFound();

        _service.Photo.Delete(user.Photos);

        await _service.SaveAsync();
        return NoContent();
    }
}
