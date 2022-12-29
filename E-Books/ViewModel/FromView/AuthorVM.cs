using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;
public class AuthorVM
{
    [Required, StringLength(30), MinLength(10)]
    public string AuthorName { get; set; }
}
