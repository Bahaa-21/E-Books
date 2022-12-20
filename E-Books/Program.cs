using E_Books.Configurations;
using E_Books.Data;
using E_Books.IService;
using E_Books.IServices;
using E_Books.Service;
using E_Books.Services;
using E_Books.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Addind a CORS
builder.Services.AddCors();

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

//Adding a ConnectionStrings
builder.Services.AddDbContext<ApplicationDbContext>(option => 
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") , 
o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddScoped<IAuthService , AuthService>();
//Adding an AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

//Confiure the Identity
builder.Services.ConfigrureIdentity();
//Configure the JWT
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

AppDbInitializer.Seed(app);

app.UseHttpsRedirection();

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.MapDefaultControllerRoute();

app.Run();

