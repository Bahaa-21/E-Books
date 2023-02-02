using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public class AdminProfileController : ControllerBase
{
    private readonly UserManager<UsersApp> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;
    public AdminProfileController(UserManager<UsersApp> userManager,RoleManager<IdentityRole> roleManager , IUnitOfWork service , IMapper mapper) => (_userManager , _roleManager , _service  , _mapper) = (userManager , roleManager ,service , mapper);




    [HttpGet("get-profile-admin/{id}")]
    public async Task<IActionResult> GetAdminProfile(string id)
    {
        var admin = await _userManager.FindByIdAsync(id);

        if (admin is null || !await _userManager.IsInRoleAsync(admin, "Admin"))
            return BadRequest("Data sent incorrectly");


        admin = await _service.Users.Include(predicate : userId => userId.Id == id , include => include.Include(photo => photo.Photos));

        var response = _mapper.Map<UsersApp, AdminProfileVM>(admin);

        return Ok(response);
    }



    [HttpPost("upload-image/{id}")]

    public async Task<IActionResult> UploadImage(string id,[FromBody] PhotoVM photoVM)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if(user is null)
        return NotFound("This user not exiset");
        
        if(!ModelState.IsValid)
        return BadRequest(ModelState);
        
        var photo = new Photo(){
        Image = photoVM.ProfilePhoto,    
        UserId = id
        };

        await _service.Photo.AddAsync(photo);
        await _service.SaveAsync();

        return Created(nameof(UploadImage) , new {photo.Id , photo.Image});
    }
}