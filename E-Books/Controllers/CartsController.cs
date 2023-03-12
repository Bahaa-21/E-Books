using E_Books.BusinessLogicLayer.Abstract;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IUnitOfWork _service;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService ,IUnitOfWork service, IBookService bookService ,IUserService userService )
        {
            (_cartService ,_service,_bookService ,_userService) = (cartService,service, bookService,userService);  
        }

        [Authorize(Roles = "User")]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromQuery] ParamsVM param)
        {   
            var user = await _userService.GetUser();
            if(user is null)
                return Unauthorized();

           var result = await _cartService.AddItemToCart(param.BookId  , param.Amount, user.Id);
            if(!result)
                return BadRequest();
            return Ok();
        }



        // [HttpGet("get-shopping-cart-item")]
        // public async Task<IActionResult> GetShoppingCart()
        // {
        //     var item = await _shoppingCart.GetShoppingCartItems();
        //     _shoppingCart.ShoppingCartItems = item;

        //     var response = new ShoppingCartVM()
        //     {
        //         ShoppingCart = _shoppingCart,
        //         ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
        //     };
        //     return Ok(response);
        // }
        
    }
}