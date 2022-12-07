using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel;

public class AuthorVM
{
    public int Id { get; set; }

    [Required, StringLength(40)]

    public string Name { get; set; }
}