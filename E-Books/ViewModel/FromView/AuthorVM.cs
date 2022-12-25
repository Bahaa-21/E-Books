using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;
public class AuthorVM
{
    [Required , StringLength(50)]
     public string Name { get; set; }
}
public class UpdateAuthorVM : AuthorVM
{
  public int Id {get; set;}
  public ICollection<int> Books {get; set;}
}