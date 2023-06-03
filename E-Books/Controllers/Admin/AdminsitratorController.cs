using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Admin;

[ApiController]
// [Authorize(Roles = "Super Admin")]
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
        var users = await _service.Users.GetAllAsync(requestParams, include: inc => inc.Include(p => p.Photo));

        if (users is null)
            return BadRequest();
        var response = _mapper.Map<IEnumerable<UserDetailsVM>>(users);

        return Ok(new { response, PageCount = users.PageCount });
    }

    [HttpGet]
    [Route("api/get-all-admins")]
    public async Task<IActionResult> GetAllAdmins()
    {
        var admins = await _authService.GetAllAdmins();

        if (admins is null)
            return BadRequest();

        var response = _mapper.Map<IEnumerable<UserDetailsVM>>(admins);
        return Ok(response);
    }


    [HttpGet]
    [Route("api/get-roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _authService.GetAllRolesAsync();
        return Ok(_mapper.Map<IList<RolesVM>>(roles));
    }

    [HttpPost]
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

    [HttpPut]
    [Route("api/update-role/{id}")]
    public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] RolesVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var role = await _authService.GetRoleAsync(id);

        if (role is null)
            return NotFound();

        _mapper.Map(model, role);

        var result = await _authService.UpdateRoleAsync(role);
        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(_mapper.Map(role, model));
    }


    [HttpDelete]
    [Route("api/delete-role/{id}")]
    public async Task<IActionResult> DeleteRoleAsync(string id)
    {
        var result = await _authService.DeleteRoleAsync(id);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);
        return Ok();
    }
}
