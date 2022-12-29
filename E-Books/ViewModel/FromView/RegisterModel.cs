using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;

public class RegisterModel
{
    [Required, StringLength(30), MinLength(10)]
    public string FirstName { get; set; }
    [Required, StringLength(30), MinLength(10)]
    public string LastName { get; set; }
    [Required, StringLength(50)]
    public string Email { get; set; }
    [Required, StringLength(50)]
    public string Password { get; set; }
}

