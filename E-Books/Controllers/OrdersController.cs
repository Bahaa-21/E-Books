using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers;

[ApiController]
[Route("api/[controller]")]
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



    [HttpGet("get-order-user")]
    public async Task<IActionResult> GetOrderUser()
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

        var cartId = await _service.Carts.GetAsync(predicate: c => c.UserId == user.Id, null);

        var order = await _orderService.StoreOrderAsync(cartId.Id, user.Id, user.Address, user.Email, user.UserName, user.PhoneNumber);

        await _cartService.ClearCartUserItems(cartId.Id);

        await _service.SaveAsync();

        var response = _mapper.Map<OrderItemsVM>(order);

        return Created(nameof(CompleteOrder), new { response, statusCode = StatusCodes.Status201Created });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("get-all-orders")]
    public async Task<IActionResult> GetAllOrders([FromQuery] RequestParams requestParams)
    {
        var orders = await _service.Orders.GetAllAsync(requestParams, include: inc => inc.Include(o => o.OrderItems).ThenInclude(b => b.Books));

        var response = _mapper.Map<IList<OrderItemsVM>>(orders);

        return Ok(response);
    }
}
