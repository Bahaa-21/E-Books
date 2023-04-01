using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Admin;

[ApiController]

public class AdminsitratorController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IUserService _userService;
    

    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public AdminsitratorController(IUnitOfWork service,
                                  IAuthService authService,
                                  IUserService userService,
                                  IAuthService authService1,
                                  IMapper mapper) =>
        (_service, _userService, _authService, _mapper) = (service, userService, authService1, mapper);


    [HttpGet]
    [Route("api/get-users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] RequestParams requestParams)
    {
        var users = await _service.Users.GetAllAsync(requestParams, include: inc => inc.Include(p => p.Photos));

        if (users is null)
            return BadRequest();
        var response = _mapper.Map<IEnumerable<UserProfileVM>>(users);

        return Ok( new { response , PageCount = users.PageCount});
    }




    [HttpPost()]
    [Route("api/add-role")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddRoleAsync(model);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(result);
    }
}
