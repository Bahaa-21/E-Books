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
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    public AdminProfileController(UserManager<UsersApp> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork service, IAuthService authService, IMapper mapper) =>
    (_userManager, _roleManager, _service, _authService, _mapper) = (userManager, roleManager, service, authService, mapper);




    [HttpGet("get-profile-admin/{id}")]
    public async Task<IActionResult> GetProfile(string id)
    {
        var admin = await _userManager.FindByIdAsync(id);

        if (admin is null || !await _userManager.IsInRoleAsync(admin, "Admin"))
            return BadRequest($"Submitted data is invalid ,{ModelState}");


        admin = await _service.Users.Include(userId => userId.Id == id, include => include.Include(ph => ph.Photos));

        var response = _mapper.Map<UsersApp, UserProfileVM>(admin);

        return Ok(response);
    }




    [HttpPost("upload-image/{id}")]

    public async Task<IActionResult> UploadImage(string id, [FromBody] PhotoVM photoVM)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return NotFound($"This user not exiset");

        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");

        var photo = new Photo()
        {
            Image = photoVM.ProfilePhoto,
            UserId = id
        };

        await _service.Photo.AddAsync(photo);
        await _service.SaveAsync();

        return Created(nameof(UploadImage), new { photo.Id, photo.Image });
    }



    [HttpDelete("remove-image/{id}")]
    public async Task<IActionResult> RemoveImageAsync(int id)
    {

        var img = await _service.Photo.GetAsync(predicate: photo => photo.Id == id, null);

        if (img is null)
            return BadRequest();

        _service.Photo.Delete(img);

        await _service.SaveAsync();
        return NoContent();
    }




    [HttpPatch("update-profile")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileVM adminProfileVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var user = await _userManager.FindByEmailAsync(adminProfileVM.Email);
        if (user is null)
            return NotFound();

        _mapper.Map(adminProfileVM, user);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            string errors = string.Empty;
            foreach (var error in result.Errors)
                errors += $"{error.Description} ,";

            return BadRequest(errors);
        }

        return Ok();
    }
}