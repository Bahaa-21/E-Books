using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Books.DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext<UsersApp>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Book_Author>().HasKey(sec => new { sec.AuthorId, sec.BookId });

        builder.Entity<Book_Author>()
        .HasOne(b => b.Books)
        .WithMany(ba => ba.Authors)
        .HasForeignKey(f => f.BookId);

        builder.Entity<Book_Author>()
        .HasOne(b => b.Authors)
        .WithMany(ba => ba.Books)
        .HasForeignKey(f => f.AuthorId);

        builder.Entity<CartBook>().HasKey(sec => new {sec.BookId , sec.CartId});

        builder.Entity<CartBook>()
        .HasOne(book => book.Books)
        .WithMany(carts => carts.CartBooks)
        .HasForeignKey(fr => fr.BookId);

         builder.Entity<CartBook>()
        .HasOne(cart => cart.Carts)
        .WithMany(carts => carts.CartBooks)
        .HasForeignKey(fr => fr.CartId);

        builder.Entity<CartBook>().Property(p => p.AddedOn).HasDefaultValue(DateTime.Now);
        builder.Entity<CartBook>().Property(p => p.Amount).HasDefaultValue(1);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    public DbSet<BookLanguage> BooksLanguages { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Book_Author> BooksAuthors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Carts> Carts { get; set; }
}