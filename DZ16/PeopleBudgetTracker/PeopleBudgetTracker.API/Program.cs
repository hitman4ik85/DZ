using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PeopleBudgetTracker.API.Endpoints;
using PeopleBudgetTracker.Core.Interfaces;
using PeopleBudgetTracker.Core.Services;
using PeopleBudgetTracker.Storage;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Додаємо конфігурацію з appsettings.json
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Підключення до бази даних
builder.Services.AddDbContext<PeopleBudgetTrackerContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Додаємо AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Додаємо підтримку JWT аутентифікації
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration["AppSettings:Token"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapTransactionEndpoints();
app.MapCategoryEndpoints();

app.Run();
