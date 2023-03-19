using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Client;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;
    private readonly IOrdersService _orderService;
    private readonly IMapper _mapper;
    public OrdersController(IUnitOfWork service,
                            IUserService userService,
                            ICartService cartService,
                            IOrdersService orderService,
                            IMapper mapper) =>
    (_service, _userService, _cartService, _orderService, _mapper) = (service, userService, cartService, orderService, mapper);





    [Authorize(Roles ="User")]
    [HttpPost("make-order")]
    public async Task<IActionResult> MakeOrder()
    {
        var user = await _userService.GetUser();

        var cartUser = await _service.Carts.GetAsync(predicate: c => c.UserId == user.Id, null);

        var carts = await _service.CartBooks.GetAllAsync(predicate: c => c.CartId == cartUser.Id, inc => inc.Include(b => b.Books));

        var result = await _orderService.StoreOrderAsync(carts, user);
        if (!result)
            return BadRequest();

        await _cartService.ClearCartUserItems(cartUser.Id);
        await _service.SaveAsync();

        return Created(nameof(MakeOrder), "Order completed successfully");
    }
}
