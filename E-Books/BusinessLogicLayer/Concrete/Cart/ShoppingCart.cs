using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Books.BusinessLogicLayer.Cart;
public class ShoppingCart
{
    public ApplicationDbContext _context { get; set; }

    public ShoppingCart(ApplicationDbContext context) => _context = context;

    public List<ShoppingCartItems> ShoppingCartItems { get; set; }
    public string ShoppingCartId { get; set; }


    public List<ShoppingCartItems> GetShoppingCartItems() => ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(s => s.ShoppingCartItemId == ShoppingCartId).Include(n => n.Book).ToList());

    public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(s => s.ShoppingCartItemId == ShoppingCartId).Select(p => p.Book.Price * p.Amount).Sum();

    public async void AddItemToCart(Book book)
    {
        var shoppingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(n => n.Book.Id == book.Id && n.ShoppingCartItemId == ShoppingCartId);

        if (shoppingCartItem is null)
        {

            shoppingCartItem = new ShoppingCartItems()
            {
                ShoppingCartItemId = ShoppingCartId,
                Book = book,
                Amount = 1
            };
            await _context.ShoppingCartItems.AddAsync(shoppingCartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }
        await _context.SaveChangesAsync();
    }

    public async void RemoveItemFromCart(Book book)
    {
        var shoppingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(n => n.Book.Id == book.Id && n.ShoppingCartItemId == ShoppingCartId);

        if (shoppingCartItem != null)
        {
            if (shoppingCartItem.Amount > 1)
            {
                shoppingCartItem.Amount--;
            }
            else
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);
            }
        }
        _context.SaveChanges();
    }
}