using E_Books.Models;

namespace E_Books.Data;

public class AppDbInitializer
{
    public static async void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            if (!context.Publishers.Any())
            {
                await context.Publishers.AddRangeAsync(
                    new Publisher()
                    {
                        Name = "Dar Alnwar"
                    },
                    new Publisher()
                    {
                        Name = "Kalemat"
                    },
                    new Publisher()
                    {
                        Name = "Afkar"
                    }
                );
                await context.SaveChangesAsync();
            }
            if (!context.BooksLanguages.Any())
            {
                await context.AddRangeAsync(
                    new BookLanguage()
                    {
                        LanguageName = "English"
                    },
                    new BookLanguage()
                    {
                        LanguageName = "Arabic"
                    },
                    new BookLanguage()
                    {
                        LanguageName = "France"
                    });
                await context.SaveChangesAsync();
            }
            if (!context.Authors.Any())
            {
                await context.AddRangeAsync(
                    new Author()
                    {
                        Name = "Adham Alsharqawi"
                    },
                     new Author()
                     {
                         Name = "Paolo"
                     },
                      new Author()
                      {
                          Name = "Smaith"
                      });
                await context.SaveChangesAsync();
            }
            if (!context.Books.Any())
            {
                await context.AddRangeAsync(
                    new Book()
                    {
                        Title = "ليطمئن قلبي",
                        Description = "Text ....",
                        NumberPages = 377,
                        Price = 15000,
                        IsFree = false,
                        PublicationDate = DateTime.UtcNow.AddMonths(4),
                        PublisherId = 1,
                        LanguagesId = 2,
                        GenreId = 1
                    },
                    new Book()
                    {
                        Title = "Alkhemyaee",
                        Description = "Text ....",
                        NumberPages = 150,
                        Price = 22000,
                        IsFree = false,
                        PublicationDate = DateTime.UtcNow.AddMonths(2),
                        PublisherId = 2,
                        LanguagesId = 1,
                        GenreId = 2 
                    },
                    new Book()
                    {
                        Title = "The Grapes of wrath",
                        Description = "Text ....",
                        NumberPages = 189,
                        Price = 20000,
                        IsFree = false,
                        PublicationDate = DateTime.UtcNow.AddMonths(1),
                        PublisherId = 3,
                        LanguagesId = 1,
                        GenreId = 3
                    });
                
                await context.SaveChangesAsync();
            }
            if (!context.BooksAuthors.Any())
            {
                await context.AddRangeAsync(
                    new Book_Author()
                    {
                        BookId = 1,
                        AuthorId = 1,
                    },
                    new Book_Author()
                    {
                        BookId = 2,
                        AuthorId = 2,
                    },
                    new Book_Author()
                    {
                        BookId = 3,
                        AuthorId = 3,
                    });
                await context.SaveChangesAsync();
            }

        }

    }

}
