using DZ6.Core.Interfaces;
using DZ6.Core.Services;
using DZ6.Storage.Repositories;
using DZ6.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//тут будемо робити як раз таки ДепенденсиІнжекшени, спершу нам потрібно проініціалізувати через метод наш DbContext
//ініціалізуємо наш DZ6Context та передаємо опшенси(opt), які приймає наш конструктор і скажемо opt.UseSqlServer,
//для того, що нам потрібно використовувати наш власний SQL Server, та передамо ссилку, щоб під'єднатися до нашої Бази Даних
builder.Services.AddDbContext<DZ6Context>(
    opt => opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DZ6DB;Integrated Security=True;"));

builder.Services.AddScoped<IRepository, GenericRepository>(); //підв'язуємо репозиторій, та всюди де видно наш IRepository, використовуй GenericRepository
builder.Services.AddScoped<IProductService, ProductService>(); //далі сервіси
builder.Services.AddScoped<IClientService, ClientService>();

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