using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public int NumberPages { get; set; }
    [Required]
    public double Price { get; set; }
    public DateTime PublicationDate { get; set; }

    public string Image { get; set; }

    [Required]
    public int PublisherId { get; set; }
    [ForeignKey("PublisherId")]
    public virtual Publisher Publishers { get; set; }
    [Required]
    public int LanguagesId { get; set; }
    [ForeignKey("LanguagesId")]
    public virtual BookLanguage Languages { get; set; }

    public int? GenreId { get; set; }
    [ForeignKey("GenreId")]
    public virtual Genre Genres { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual ICollection<Book_Author> Authors { get; set; }

    public Book()
    {
        Authors = new Collection<Book_Author>();
        OrderItems = new Collection<OrderItem>();

    }

}