using Microsoft.EntityFrameworkCore;
using FoodOrderApi.Data;
using FoodOrderApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Local");
builder.Services.AddDbContext<FoodOrderContext>(opt =>
    opt.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    Console.WriteLine("Hello from Middleware!");
    await next.Invoke(context);
});

app.AddUserEndpoints();
app.AddProductEndpoints();
app.AddOrderEndpoints();

app.Run();