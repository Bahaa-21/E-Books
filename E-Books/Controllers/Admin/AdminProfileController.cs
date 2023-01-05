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

    [HttpGet]
    public async Task<IActionResult> GetAdminProfile(string id)
    {
        var admin = await _userManager.FindByIdAsync(id);
        
        var response = _mapper.Map<UsersApp , AdminProfileVM>(admin);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(string id,[FromForm] PhotoVM photoVM)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if(user is null)
        return NotFound();

        using var dataStream = new MemoryStream();
        await photoVM.ProfilePhto.CopyToAsync(dataStream);

         
        var photo = _mapper.Map<PhotoVM , Photo>(photoVM);
        photo.AddedOn = DateTime.UtcNow;
        photo.ProfilePhto = dataStream.ToArray();
        return Ok();
    }
}