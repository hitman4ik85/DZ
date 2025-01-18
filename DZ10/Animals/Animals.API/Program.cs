using Animals.API.Filters;
using Animals.API.Interfaces;
using Animals.API.Middlewares;
using Animals.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Реєстрація сервісів
builder.Services.AddSingleton<IAnimalService, AnimalService>();

// Додаємо фільтри
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new ErrorHandlerFilter());
    opt.Filters.Add(new AuthExceptionFilter());
    opt.Filters.Add(new ExecutionTimeFilter());
    opt.Filters.Add(new HttpLogFilter());
    opt.Filters.Add(new LogFilter());
});

// Реєстрація Middleware
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.AddTransient<AuthCheckMiddleware>();
builder.Services.AddTransient<AnimalMiddleware>();
builder.Services.AddTransient<RequestResponseInfoMiddleware>();

// Додаємо контролери та інші сервіси
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

// Middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<AuthCheckMiddleware>();
app.UseMiddleware<AnimalMiddleware>();
app.UseMiddleware<RequestResponseInfoMiddleware>();

app.MapControllers();
app.Run();
