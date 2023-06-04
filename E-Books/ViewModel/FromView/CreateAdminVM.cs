using System.ComponentModel.DataAnnotations;
using E_Books.DataAccessLayer.Enum;

namespace E_Books.ViewModel.FromView;

public class CreateAdminVM
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public string Role { get; set; }
}
