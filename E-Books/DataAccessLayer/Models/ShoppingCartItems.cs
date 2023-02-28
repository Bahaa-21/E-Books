namespace E_Books.DataAccessLayer.Models;

public class ShoppingCartItems
{
    public int Id { get; set; }

    public int Amount { get; set; }
    public Book Book { get; set; }

    public string ShoppingCartItemId { get; set; }
}