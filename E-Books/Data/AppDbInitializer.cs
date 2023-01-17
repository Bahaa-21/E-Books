using E_Books.Data;
using E_Books.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Books.Data;

public class AppDbInitializer
{
    public static async void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            #region Genre Table
            if (!context.Genres.Any())
            {
                await context.Genres.AddRangeAsync(
                    new Genre()
                    {
                        Name = "Genre1"
                    },
                     new Genre()
                     {
                         Name = "Genre2"
                     },
                     new Genre()
                     {
                         Name = "Genre3"
                     },
                     new Genre()
                     {
                         Name = "Genre4"
                     },
                     new Genre()
                     {
                         Name = "Genre5"
                     },
                     new Genre()
                     {
                         Name = "Genre6"
                     }
                );
            }
            #endregion

            #region Publisher Table
            if (!context.Publishers.Any())
            {
                await context.Publishers.AddRangeAsync(
                    new Publisher()
                    {
                        Name = "Publisher1"
                    },
                    new Publisher()
                    {
                        Name = "Publisher2"
                    },
                    new Publisher()
                    {
                        Name = "Publisher3"
                    },
                    new Publisher()
                    {
                        Name = "Publisher4"
                    }, new Publisher()
                    {
                        Name = "Publisher5"
                    }, new Publisher()
                    {
                        Name = "Publisher6"
                    }, new Publisher()
                    {
                        Name = "Publisher7"
                    }
                );
                await context.SaveChangesAsync();
            }
            #endregion

            #region Langauge Table
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
            #endregion

            #region Author Table
            if (!context.Authors.Any())
            {
                await context.AddRangeAsync(
                    new Author()
                    {
                        Name = "Author1"
                    },
                     new Author()
                     {
                         Name = "Author2"
                     },
                      new Author()
                      {
                          Name = "Author3"
                      },
                      new Author()
                      {
                          Name = "Author4"
                      },
                      new Author()
                      {
                          Name = "Author5"
                      },
                      new Author()
                      {
                          Name = "Author6"
                      });

                await context.SaveChangesAsync();
            }
            #endregion

            #region Book Table
            if (!context.Books.Any())
            {
                await context.AddRangeAsync(
                    new Book()
                    {
                        Title = "Book1",
                        Description = "Text ....",
                        NumberPages = 377,
                        Image = "String.......",
                        Price = 15000,
                        PublicationDate = DateTime.UtcNow.AddMonths(4),
                        PublisherId = 1,
                        LanguagesId = 2,
                        GenreId = 1,
                    },
                    new Book()
                    {
                        Title = "Alkhemyaee",
                        Description = "Text ....",
                        NumberPages = 150,
                        Image = "String.......",
                        Price = 22000,
                        PublicationDate = DateTime.UtcNow.AddMonths(1),
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
                        PublicationDate = DateTime.UtcNow.AddDays(1),
                        PublisherId = 3,
                        LanguagesId = 1,
                        GenreId = 3
                    },
                      new Book()
                      {
                          Title = "Book4",
                          Description = "Text ....",
                          NumberPages = 189,
                          Image = "String.......",
                          Price = 20000,
                          PublicationDate = DateTime.UtcNow.AddDays(2),
                          PublisherId = 3,
                          LanguagesId = 1,
                          GenreId = 3
                      },
                      new Book()
                      {
                          Title = "Book5",
                          Description = "Text ....",
                          NumberPages = 189,
                          Image = "String.......",
                          Price = 20000,
                          PublicationDate = DateTime.UtcNow.AddDays(2),
                          PublisherId = 7,
                          LanguagesId = 1,
                          GenreId = 2
                      },
                      new Book()
                      {
                          Title = "Book6",
                          Description = "Text ....",
                          NumberPages = 189,
                          Image = "String.......",
                          Price = 20000,
                          PublicationDate = DateTime.UtcNow.AddDays(3),
                          PublisherId = 3,
                          LanguagesId = 1,
                          GenreId = 5
                      }, new Book()
                      {
                          Title = "Book7",
                          Description = "Text ....",
                          NumberPages = 189,
                          Image = "String.......",
                          Price = 20000,
                          PublicationDate = DateTime.UtcNow.AddDays(4),
                          PublisherId = 3,
                          LanguagesId = 1,
                          GenreId = 3
                      }

                    );

                await context.SaveChangesAsync();
            }
            #endregion

            #region Book_Author Table
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
                    },
                    new Book_Author()
                    {
                        BookId = 4,
                        AuthorId = 4,
                    },
                    new Book_Author()
                    {
                        BookId = 5,
                        AuthorId = 5,
                    },
                    new Book_Author()
                    {
                        BookId = 6,
                        AuthorId = 5,
                    },
                     new Book_Author()
                     {
                         BookId = 7,
                         AuthorId = 6,
                     });

                await context.SaveChangesAsync();
            }
            #endregion

            #region Role Table
            if (!context.Roles.Any())
            {
                await context.Roles.AddRangeAsync(
                     new IdentityRole() { Name = "User", NormalizedName = "user".ToUpper() },
                     new IdentityRole() { Name = "Admin", NormalizedName = "admin".ToUpper() },
                     new IdentityRole() { Name = "Adminsitrator", NormalizedName = "adminsitrator".ToUpper() }
                    );
                await context.SaveChangesAsync();
            }
            #endregion
           
           #region User Table
            // if(!context.Users.Any())
            // {
            //     await context.Users.AddRangeAsync
            //     (
            //         new UsersApp() 
            //         {
            //             UserName = "BahaaAtk",
            //             NormalizedUserName = "BahaaAtk".ToUpper(),
            //             FirstName = "Bahaa",
            //             LastName = "Atekah",
            //             Email = "bahaa@gmail.com",
            //             NormalizedEmail = "Bahaa@gmail.com".ToUpper(),
            //             EmailConfirmed =true,
            //             Gender = 0,
            //             PhoneNumber = "+963 951584338",
            //         }
            //      );
            // }
            #endregion
        }
    }
}