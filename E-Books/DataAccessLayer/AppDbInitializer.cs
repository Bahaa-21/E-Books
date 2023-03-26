using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Books.DataAccessLayer;

public class AppDbInitializer
{
    public static async void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var UserManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<UsersApp>>();

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
                await context.BooksLanguages.AddRangeAsync(
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
                await context.Authors.AddRangeAsync(
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
            if (!context.Users.Any())
            {
                UsersApp admin = new()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@safa7at.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin@safa7at.com",
                    Gender = Enum.Gender.Male,
                    Address = "Damascuse,Syria",
                    PhoneNumber = "0951584338",
                };
                await UserManager.CreateAsync(admin, "@dmin123");
                await UserManager.AddToRoleAsync(admin, "Admin");

                UsersApp user = new()
                {
                    FirstName = "Bahaa",
                    LastName = "Atekah",
                    Email = "bahaa@safa7at.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "bahaa@safa7at.com",
                    Gender = Enum.Gender.Male,
                    Address = "Damascuse,Syria",
                    PhoneNumber = "0951584338",
                };
                await UserManager.CreateAsync(user, "P@ssword123");
                await UserManager.AddToRoleAsync(admin, "User");
            }
            #endregion
        }
    }
}