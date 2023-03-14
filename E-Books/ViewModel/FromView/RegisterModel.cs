using System.ComponentModel.DataAnnotations;
using E_Books.DataAccessLayer.Enum;

namespace E_Books.ViewModel.FromView;

public class RegisterModel
{
    [Required, StringLength(30)]
    public string FirstName { get; set; }
    [Required, StringLength(30)]
    public string LastName { get; set; }
    [Required, StringLength(50) ,DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, StringLength(50) , DataType(DataType.Password)]
    public string Password { get; set; }
    [Required,StringLength(13) , DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    [Required]
    public Gender Gender { get; set; }
}

