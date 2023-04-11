using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Client;

[ApiController]
[Route("api/")]
[Authorize]
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



    [HttpGet("get-user-orders")]
    public async Task<IActionResult> GetUserOrder()
    {
        var user = await _userService.GetUser();
        var orders = await _service.Orders.GetAllAsync(or => or.UserId == user.Id, include: inc => inc.Include(o => o.OrderItems).ThenInclude(b => b.Books));
        var response = _mapper.Map<IList<OrderItemsVM>>(orders);
        return Ok(response);
    }



    [HttpPost("make-order")]
    public async Task<IActionResult> CompleteOrder()
    {
        var user = await _userService.GetUser();

        var cartUser = await _service.Carts.GetAsync(predicate: c => c.UserId == user.Id, null);

        var order = await _orderService.StoreOrderAsync(cartUser.Id, user.Id, user.Address, user.Email);

        await _cartService.ClearCartUserItems(cartUser.Id);

        await _service.SaveAsync();

        return Created(nameof(CompleteOrder), new { message = "Order completed successfully" , statusCode = StatusCodes.Status201Created});
    }
}
