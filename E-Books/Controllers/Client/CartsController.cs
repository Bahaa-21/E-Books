using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _service;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartsController(ICartService cartService,
                               IUnitOfWork service,
                               IUserService userService,
                               IMapper mapper) =>
        (_cartService, _service, _userService, _mapper) = (cartService, service, userService, mapper);


        [Authorize(Roles = "User")]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromQuery] ParamsVM param)
        {
            var user = await _userService.GetUser();
            if (user is null)
                return Unauthorized();

            var result = await _cartService.AddItemToCart(user.Id, param.BookId, param.Amount);
            if (!result)
                return BadRequest();

            await _service.SaveAsync();
            return Created(nameof(AddToCart), StatusCodes.Status201Created);
        }


        [Authorize(Roles = "User")]
        [HttpGet("get-cart-items")]
        public async Task<IActionResult> GetShoppingCart()
        {
            var user = await _userService.GetUser();
            if (user is null)
                return Unauthorized();

            var cartUser = await _service.Carts.GetAsync(predicate: c => c.UserId == user.Id, null);
            if (cartUser is null)
                return NotFound("You don't have a cart");

            var carts = await _service.CartBooks.GetAllAsync(predicate: c => c.CartId == cartUser.Id, inc => inc.Include(b => b.Books));
            if (carts.Count == 0)
                return NotFound("You don't have prudoct in your cart");

            double totalPrice = carts.Select(c => c.Books.Price * c.Amount).Sum();

            var response = _mapper.Map<IEnumerable<CartsVM>>(carts);
            
            return Ok(new { response, totalPrice });
        }

        [Authorize(Roles = "User")]
        [HttpDelete("remove-item-from-cart/{bookId:int}")]
        public async Task<IActionResult> RemoveItemFromCart(int bookId)
        {
            var user = await _userService.GetUser();
            if (user is null)
                return Unauthorized();

            var cartUser = await _service.Carts.GetAsync(predicate: c => c.UserId == user.Id, null);
            if (cartUser is null)
                return NotFound();

            var result = await _cartService.RemoveItemFromCart(bookId, cartUser.Id);

            if (!result)
                return BadRequest();

            await _service.SaveAsync();
            return NoContent();
        }
    }
}