namespace E_Books.DataAccessLayer.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<Book> Books { get; set; }
    public Genre()
    {
        Books = new List<Book>();
    }
}
