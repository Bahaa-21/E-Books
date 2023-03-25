using System.Collections.ObjectModel;

namespace E_Books.ViewModel.ToView;
public class ReadBookVM
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }
    
    public int NumberPages { get; set; }

    public double Price { get; set; }
    public string Image { get; set; }
    public string PublicationDate { get; set; }

    public string Publishers { get; set; }

    public string GenreType {get; set;}
    
    public string Language { get; set; }

    public List<string> Authors { get; set; }

}