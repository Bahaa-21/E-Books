using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Books.DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext<UsersApp>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Relationships Many-to-Many
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

        builder.Entity<CartBook>().Property(p => p.Amount).HasDefaultValue(1);
        #endregion    

        #region Rename Table
        builder.Entity<UsersApp>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        #endregion

        
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Photo> Photos {get; set;}
    public DbSet<BookLanguage> BooksLanguages { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Book_Author> BooksAuthors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Carts> Carts { get; set; }
    public DbSet<CartBook> CartBooks {get; set;}
}