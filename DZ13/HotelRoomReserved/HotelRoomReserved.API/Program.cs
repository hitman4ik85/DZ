using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotelRoomReserved.Storage;
using HotelRoomReserved.API.Filters;
using HotelRoomReserved.API.Middlewares;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HotelRoomReservedDB;Integrated Security=True;"));

builder.Services.AddTransient<LoggingMiddleware>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value ?? string.Empty)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

// Реєстрація AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// Фільтр для обробки винятків
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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();

app.Run();
