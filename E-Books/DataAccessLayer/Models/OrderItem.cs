using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.DataAccessLayer.Models;
public class OrderItem
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public double Price { get; set; }
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public Book Books { get; set; }
    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order Orders { get; set; }
}