using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Data;

public class ApplicationDbContext : IdentityDbContext<UsersApp>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Genre>().HasData(
            new Genre()
            {
                Id = 1,
                Name = "Genre1"
            },
            new Genre()
            {
                Id = 2,
                Name = "Genre2"
            },
            new Genre()
            {
                Id = 3,
                Name = "Genre3"
            }
        );

        builder.Entity<Book_Author>().HasKey(sec => new { sec.AuthorId, sec.BookId });

        builder.Entity<Book_Author>()
        .HasOne(b => b.Books)
        .WithMany(ba => ba.Authors)
        .HasForeignKey(f => f.BookId);

        builder.Entity<Book_Author>()
        .HasOne(b => b.Authors)
        .WithMany(ba => ba.Books)
        .HasForeignKey(f => f.AuthorId);

    }

    public DbSet<Book> Books {get; set;}
    public DbSet<Author> Authors {get; set;}
   
    public DbSet<BookLanguage> BooksLanguages {get; set;}
    public DbSet<Publisher> Publishers {get; set;}
    public DbSet<Book_Author> BooksAuthors {get; set;}
    public DbSet<Genre> Genres {get; set;}
    
}