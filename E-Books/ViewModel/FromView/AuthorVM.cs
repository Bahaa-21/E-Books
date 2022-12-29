using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;
public class AuthorVM
{
    [Required , StringLength(30) , MinLength(10)]
     public string AuthorName { get; set; }
}
public class UpdateAuthorVM : AuthorVM
{
  public int Id {get; set;}
  public ICollection<int> Books {get; set;}
}