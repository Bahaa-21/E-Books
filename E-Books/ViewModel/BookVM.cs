using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel;
public class BookVM
{
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Title { get; set; }
    [Required, StringLength(150)]
    public string Description { get; set; }
    [Required]
    public int NumberPages { get; set; }
    public IFormFile Image { get; set; }
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
public class ReadBookVM
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int NumberPages { get; set; }

    public double Price { get; set; }
    public byte[] Image { get; set; }
    public DateTime PublicationDate { get; set; }

    public string Publishers { get; set; }

    public GenreVM Genres {get; set;}
    
    public string Languages { get; set; }

    public ICollection<AuthorVM> Authors { get; set; }

    public ReadBookVM()
    {
        Authors = new Collection<AuthorVM>();
    }
}

public class SearchBookVM
{
    public int Id { get; set; }

    public string Title { get; set; }

    public ICollection<string> Authors { get; set; }
}