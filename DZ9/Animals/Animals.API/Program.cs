using Animals.API.Interfaces;
using Animals.API.Middlewares;
using Animals.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAnimalService, AnimalService>();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.AddTransient<AuthCheckMiddleware>();
builder.Services.AddTransient<AnimalMiddleware>();
builder.Services.AddTransient<RequestResponseInfoMiddleware>();
builder.Services.AddTransient<CacheMiddleware>();

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

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<CacheMiddleware>();
app.UseMiddleware<AuthCheckMiddleware>();
app.UseMiddleware<AnimalMiddleware>();
app.UseMiddleware<RequestResponseInfoMiddleware>();

app.MapControllers();

app.Run();
