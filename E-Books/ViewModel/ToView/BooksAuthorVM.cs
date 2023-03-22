using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.ToView;
public class BooksAuthorVM 
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<object> BookTitle { get; set; }
}