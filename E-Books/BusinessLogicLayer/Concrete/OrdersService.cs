using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Concrete
{
    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext _context;

        public OrdersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> StoreOrderAsync(IList<CartBook> cartBooks, UsersApp user)
        {
            var order = new Order()
            {
                UserId = user.Id,
                Address = user.Address,
                Email = user.Email,
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartBooks)
            {
                OrderItem orderItem = new()
                {
                    OrderId = order.Id,
                    BookId = item.BookId,
                    Amount = item.Amount,
                    Price = item.Books.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            return true;
        }
    }
}