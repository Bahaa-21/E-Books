using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.DataAccessLayer.Models;
public class Photo
{
    public int Id { get; set; }
    [Required]
    public byte[] Image { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public UsersApp Users { get; set; }

    public Photo()
    {
        CreatedOn = DateTime.UtcNow;
    }
}