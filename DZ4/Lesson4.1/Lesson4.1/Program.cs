using Lesson4._1;
using Lesson4._1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Lesson4_1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Lesson4_1Context") ?? throw new InvalidOperationException("Connection string 'Lesson4_1Context' not found.")));

builder.Services.AddControllersWithViews();

// ============== DI ================

builder.Services.AddSingleton<IComputerService, ComputerService>();
builder.Services.AddSingleton<IDatabase, ComputerDatabase>();

// ==================================

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();