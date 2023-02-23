using System.ComponentModel.DataAnnotations;
using E_Books.Data.Enum;

namespace E_Books.ViewModel.FromView;

public class RegisterModel
{
    [Required, StringLength(30)]
    public string FirstName { get; set; }
    [Required, StringLength(30)]
    public string LastName { get; set; }
    [Required, StringLength(50)]
    public string Email { get; set; }
    [Required, StringLength(50)]
    public string Password { get; set; }
    [Required , MaxLength(13)]
    public string PhoneNumber {get; set;}
    [Required]
    public Gender Gender { get; set; }
}

