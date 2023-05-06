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
    private readonly IMapper _mapper;
    private readonly UserManager<UsersApp> _userManager;
    private readonly IUserService _userService;
    private List<string> _allowedExtenstions = new List<string> { ".jpg", ".png", ".jpeg" };
    private long _maxAllowedPosterSize = 1048576;

    public ProfileController(IUnitOfWork service,
                                 IMapper mapper,
                                 UserManager<UsersApp> userManager,
                                 IUserService userService
                                )
       => (_service, _mapper, _userManager, _userService) = (service, mapper, userManager, userService);


    [HttpGet("get-user-profile")]
    public async Task<IActionResult> GetUserProfle()
    {
        var userProfile = await _userService.GetUser();

        var response = _mapper.Map<UserProfileVM>(userProfile);

        return Ok(response);
    }




    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] PhotoVM img)
    {
        var user = await _userService.GetUser();

        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");

        if (img.Image.Length > _maxAllowedPosterSize)
            return BadRequest("Image cannot be more than 1 MB!");

        if (!_allowedExtenstions.Contains(Path.GetExtension(img.Image.FileName).ToLower()))
            return BadRequest("Only .PNG, .JPG and .JPEG images are allowed!");

        if (user.Photo != null)
        {
            _service.Photo.Delete(user.Photo);
            await _service.SaveAsync();
        }

        using var dataStream = new MemoryStream();
        await img.Image.CopyToAsync(dataStream);

        var photo = new Photo() { Image = dataStream.ToArray(), UserId = user.Id };

        await _service.Photo.AddAsync(photo);
        await _service.SaveAsync();

        var image = new DisplayPhotoVM()
        {
            ProfilePhoto = "data:image/png;base64," + Convert.ToBase64String(photo.Image)
        };

        return Ok(image);
    }


    [HttpPatch("update-user-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");

        var user = await _userService.GetUser();


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

        if (user.Photo is null)
            return NotFound();

        _service.Photo.Delete(user.Photo);

        await _service.SaveAsync();
        return NoContent();
    }
}
