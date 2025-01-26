using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Core.Services;
using ProductCatalog.Storage;
using ProductCatalog.API.Filters;
using ProductCatalog.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ProductsDB;Integrated Security=True;"));

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddTransient<PageValidationMiddleware>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ExceptionFilter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<PageValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
