using System.ComponentModel.DataAnnotations;

namespace E_Books.DataAccessLayer.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Address {get; set;}
    [Required]
    public DateTime Created {get; set;}
    public UsersApp Users{get;set;}
    public List<OrderItem> OrderItems { get; set; }
}
