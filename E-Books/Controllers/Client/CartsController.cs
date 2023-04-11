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
    [Authorize]
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


        
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromQuery] ParamsVM param)
        {
            var user = await _userService.GetUser();

            string check = _cartService.AddToCartValidation(param.BookId, param.Amount);
            if (check != "success")
                return BadRequest(check); 

            var cart = await _cartService.AddItemToCart(user.Id, param.BookId, param.Amount);
            var response = _mapper.Map<CartsVM>(cart);
            await _service.SaveAsync();

            return Created(nameof(AddToCart), new {response, statusCode = StatusCodes.Status201Created });
        }


        
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
                return NotFound("You don't have prudocts in your cart");

            double totalPrice = carts.Select(c => c.Books.Price * c.Amount).Sum();

            var response = _mapper.Map<IEnumerable<CartsVM>>(carts);
            
            return Ok(new { response, totalPrice });
        }

        
        [HttpDelete("remove-item-from-cart/{bookId:int}")]
        public async Task<IActionResult> RemoveItemFromCart(int bookId)
        {
            var user = await _userService.GetUser();
            

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