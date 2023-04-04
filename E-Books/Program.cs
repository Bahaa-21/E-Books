using E_Books.BusinessLogicLayer.Abstract;
using E_Books.BusinessLogicLayer.Concrete;
using E_Books.Configurations;
using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;
using E_Books.Settings;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers().AddNewtonsoftJson(op =>
        op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//Addind a CORS
builder.Services.AddCors();

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
    
builder.Services.AddIdentity<UsersApp , IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


//Adding a ConnectionStrings
builder.Services.AddDbContext<ApplicationDbContext>(option => 
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") , 
o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));


//Adding an AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
//Confiure the Identity
builder.Services.ConfigrureIdentity();
//Configure the JWT
builder.Services.ConfigureJWT(builder.Configuration);

//Confiure Google Authentication
builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddGoogle(option =>
                {
                    IConfigurationSection googleAuth = builder.Configuration.GetSection("Authentication:Google");
                    option.ClientId = googleAuth["ClientId"];
                    option.ClientSecret = googleAuth["ClientSecret"];
                });

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


builder.Services.AddScoped<IAuthService , AuthService>();

builder.Services.AddScoped<ICartService , CartService>();

builder.Services.AddScoped<IBookService,BookService>();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IOrdersService,OrdersService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "e-Book-Api", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Seed Data

app.UseHttpsRedirection();

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();


app.MapDefaultControllerRoute();


//Exception Handling
app.ConfigureExceptionHandler();

app.MapControllers();

AppDbInitializer.Seed(app);

app.Run();