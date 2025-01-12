using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MoviesDB;Integrated Security=True;"));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
