namespace E_Books.DataAccessLayer.Models;

public class CartBook
{
    public int CartId { get; set; }
    public Carts Carts { get; set; }

    public int BookId { get; set; }
    public Book Books { get; set; }

    public DateTime AddedOn {get; set;}
    public int Amount {get; set;}
}
