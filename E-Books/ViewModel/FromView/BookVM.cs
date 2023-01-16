using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;
public class BookVM
{
    [Required, StringLength(50)]
    public string Title { get; set; }
    [Required, StringLength(150)]
    public string Description { get; set; }
    [Required]
    public int NumberPages { get; set; }
    public string Image { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public int PublisherId { get; set; }
    [Required]
    public int LanguagesId { get; set; }
    [Required]
    public int GenreId { get; set; }
    [Required]
    public ICollection<int> Authors { get; set; }

}