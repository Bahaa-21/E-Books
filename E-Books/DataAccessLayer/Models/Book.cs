using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace E_Books.DataAccessLayer.Models;

public class Book
{

    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string Title { get; set; }
    [Required, MaxLength(150)]
    public string Description { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public int NumberPages { get; set; }
    [Required]
    public double Price { get; set; }
    public DateTime PublicationDate { get; set; }
    [Required]
    public string Image { get; set; }

    [Required]
    public int PublisherId { get; set; }
    [ForeignKey("PublisherId")]
    public Publisher Publishers {get; set;}
    [Required]
    public int LanguagesId { get; set; }
    [ForeignKey("LanguagesId")]
    public  BookLanguage Languages { get; set; }

    public int? GenreId { get; set; }
    [ForeignKey("GenreId")]
    public  Genre Genres { get; set; }

    public  List<OrderItem> OrderItems { get; set; }
    public  ICollection<Book_Author> Authors { get; set; }
    public ICollection<CartBook> CartBooks { get; set; }

    public Book()
    {
        PublicationDate = DateTime.UtcNow;
        Authors = new Collection<Book_Author>();
        CartBooks = new Collection<CartBook>();
        OrderItems = new List<OrderItem>();
    }
}