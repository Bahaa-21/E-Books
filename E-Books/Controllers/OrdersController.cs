using E_Books.BusinessLogicLayer.Abstract;
using E_Books.BusinessLogicLayer.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="User")]
    public class OrdersController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IUnitOfWork _service;
        private readonly ShoppingCart _shoppingCart;
        public OrdersController(IUnitOfWork service, IBookService bookService , ShoppingCart shoppingCart) => (_service,_bookService,_shoppingCart) = (service, bookService,shoppingCart);


        
    }
}