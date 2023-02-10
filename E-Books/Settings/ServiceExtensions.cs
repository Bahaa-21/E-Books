using E_Books.Data;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
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
            opt.Password.RequiredUniqueChars = 0;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);

        builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
    }


    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
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
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidateAudience = true,
                 ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
             };
         });
    }


    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                var contextRequest = context.Features.Get<IHttpRequestFeature>();

                if (contextFeature is not null)
                {
                    await context.Response.WriteAsync(new ErrorVM()
                    {
                        StatusCode = context.Response.StatusCode,
                        Masseage = contextFeature.Error.Message,
                        Path = contextRequest.Path,
                    }.ToString());
                }
            });
        });
    }
}