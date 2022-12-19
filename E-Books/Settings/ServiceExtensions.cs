using E_Books.Data;
using E_Books.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace E_Books.Settings;

public static class ServiceExtensions
{
    public static void ConfigrureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<UsersApp>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireUppercase = false;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);

        builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
    }


    public static void ConfigureJWT(this IServiceCollection services , IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT");
        var key = configuration.GetSection("JWT:Key").Value;

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(opt =>
         {
             opt.TokenValidationParameters = new TokenValidationParameters()
             {
                 ValidateIssuer = true,
                 ValidateLifetime= true,
                 ValidateIssuerSigningKey = true,
                 ValidateAudience= true,
                 ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
             };
         });
    }
}
