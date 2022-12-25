using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel;

public class RegisterModel
{
    [Required, StringLength(100)]
    public string FirstName { get; set; }
    [Required, StringLength(100)]

    public string LastName { get; set; }
    [Required, StringLength(50)]
    public string Email { get; set; }
    [Required, StringLength(50)]
    public string Password { get; set; }
}

