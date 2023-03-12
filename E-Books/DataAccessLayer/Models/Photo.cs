using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.DataAccessLayer.Models;
public class Photo
{
    public int Id { get; set; }
    [Required]
    public string Image { get; set; }
    public DateTime AddedOn { get; set; }
    [Required]
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public UsersApp Users { get; set; }

    public Photo()
    {
        AddedOn = DateTime.UtcNow;
    }
}