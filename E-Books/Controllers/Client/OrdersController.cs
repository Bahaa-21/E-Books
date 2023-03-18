using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers.Client;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public OrdersController(IUnitOfWork service,
                            IUserService userService,
                            IMapper mapper) =>
    (_service , _userService , _mapper) = (service , userService , mapper);

    [HttpPost("make-order")]
    public async Task<IActionResult> MakeOrder()
    {
        var user = await _userService.GetUser();
        return Ok();
    }
}