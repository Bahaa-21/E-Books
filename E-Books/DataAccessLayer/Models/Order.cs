using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.DataAccessLayer.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [ForeignKey("UserId")]
    public UsersApp Users { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public Order()
    {
        Created = DateTime.UtcNow;
        OrderItems = new List<OrderItem>();
        Status = "Under review";
    }
}