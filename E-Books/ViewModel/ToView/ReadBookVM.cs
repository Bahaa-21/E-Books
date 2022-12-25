using System.Collections.ObjectModel;

namespace E_Books.ViewModel.ToView;
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

    public IList<object> Authors { get; set; }

}