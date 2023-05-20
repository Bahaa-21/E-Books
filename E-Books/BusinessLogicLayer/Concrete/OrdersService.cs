using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.EntityFrameworkCore;

namespace E_Books.BusinessLogicLayer.Concrete
{
    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext _context;

        public OrdersService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Order> StoreOrderAsync(int cartId, string userId, string userAddress, string userEmail , string userName, string phoneNumber)
        {
            var cartBooks = await _context.CartBooks.Where(c => c.CartId == cartId).Include(b => b.Books).ToListAsync();
            var order = new Order()
            {
                UserId = userId,
                UserName = userName,
                PhoneNumber = phoneNumber,
                Address = userAddress,
                Email = userEmail
            };
            await _context.Orders.AddAsync(order);
            
            foreach (var item in cartBooks)
            {
                OrderItem orderItem = new()
                {
                    OrderId = order.Id,
                    BookId = item.BookId,
                    Amount = item.Amount,
                    Price = item.Books.Price,
                };
                order.OrderItems.Add(orderItem);
            }
            return order;
        }
    }
}