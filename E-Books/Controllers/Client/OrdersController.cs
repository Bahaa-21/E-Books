using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
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



    [HttpGet("get-user-orders")]
    public async Task<IActionResult> GetUserOrder()
    {
       var user = await _userService.GetUser();
       var order = await _service.Orders.GetAsync(or => or.UserId == user.Id , include : null);

       var orderItems = await _service.OrderItems.GetAllAsync(or => or.OrderId == order.Id , include : inc => inc.Include(b => b.Books));
       double totatPrice = orderItems.Select(s => s.Books.Price * s.Amount).Sum();
       var response = _mapper.Map<IEnumerable<OrderItemsVM>>(orderItems);
       return Ok(new{response , totatPrice });
    }



    [Authorize(Roles ="User")]
    [HttpPost("make-order")]
    public async Task<IActionResult> CompleteOrder()
    {
        var user = await _userService.GetUser();

        var cartUser = await _service.Carts.GetAsync(predicate: c => c.UserId == user.Id, null);

        var result = await _orderService.StoreOrderAsync(cartUser.Id, user.Id , user.Address , user.Email);
        if (!result)
            return BadRequest();

        await _cartService.ClearCartUserItems(cartUser.Id);
        
        await _service.SaveAsync();

        return Created(nameof(CompleteOrder), "Order completed successfully");
    }
}
