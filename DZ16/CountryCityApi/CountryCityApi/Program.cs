using Microsoft.EntityFrameworkCore;
using CountryCityApi.Data;
using CountryCityApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Local");
builder.Services.AddDbContext<CountryCityContext>(opt =>
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

app.AddCountryEndpoints();
app.AddCityEndpoints();
app.AddHotelEndpoints();

app.Run();
