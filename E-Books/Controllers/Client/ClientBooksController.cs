using AutoMapper;
using E_Books.DataAccessLayer;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace E_Books.Controllers.Client;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class ClientBooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly UserManager<UsersApp> _userManager;
    private readonly IBookService _bookService;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    private readonly IMapper _mapper;
    public ClientBooksController(IUnitOfWork service, IBookService bookService, IMapper mapper, IAuthService authService, UserManager<UsersApp> userManager, IUserService userService) =>
    (_service, _bookService, _mapper, _authService, _userManager ,_userService) = (service, bookService, mapper, authService, userManager,userService);




    [HttpGet("get-user-profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userProfile = await _userService.GetUserProfile();

        var response = _mapper.Map<UserProfileVM>(userProfile);

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


    [HttpPatch("update-user-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var user = await _userService.GetUserProfile();

        if (user is null)
            return Unauthorized();

        _mapper.Map(model, user);

        var result = await _userService.UpdateProfile(user);

        if (!result)
            return BadRequest(user);

        var response = _mapper.Map<UpdateProfileVM>(user); 

        return Ok(response);
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






}
