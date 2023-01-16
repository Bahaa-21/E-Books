using AutoMapper;
using E_Books.IServices;
using E_Books.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public class AdminProfileController : ControllerBase
{
    private readonly UserManager<UsersApp> _userManager;
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;
    public AdminProfileController(UserManager<UsersApp> userManager, IUnitOfWork service , IMapper mapper) => (_userManager, _service , _mapper) = (userManager, service , mapper);




    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdminProfile(string id)
    {
        var admin = await _userManager.FindByIdAsync(id);
        var response = _mapper.Map<UsersApp , AdminProfileVM>(admin);
        return Ok(response);
    }



    [HttpPost("{id}")]
    public async Task<IActionResult> UploadImage(string id,[FromBody] PhotoVM photoVM)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if(user is null)
        return NotFound();

        var img = Convert.FromBase64String(photoVM.ProfilePhto);

        var photo = new Photo()
        {
        ProfilePhoto = img,
        AddedOn = DateTime.UtcNow,
        UsersAppId = user.Id
        };

        await _service.Photo.AddAsync(photo);
        await _service.SaveAsync();


        return Created(nameof(UploadImage) , photo);
    }
}