namespace E_Books.Models;

public class Genre
{
    public  int Id { get; set; }
    public string Name { get; set; }

    public IList<Book> Books {get; set;}
    public Genre()
    {
        this.Books = new List<Book>();
    }
}
