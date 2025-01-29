using Microsoft.AspNetCore.Mvc;
using FoodOrderApi.Data;
using FoodOrderApi.Models;

namespace FoodOrderApi.Endpoints;

public static class UserEndpoints
{
    public static WebApplication AddUserEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/users/");

        // Отримати всіх користувачів + пагінація
        endpoint.MapGet("/", (int page, int pageSize, FoodOrderContext context) =>
        {
            var users = context.Users
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            return Results.Ok(users);
        });

        // Отримати користувача за ID
        endpoint.MapGet("{id}", ([FromRoute] int id, FoodOrderContext context) =>
        {
            var user = context.Users.Find(id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        });

        // Пошук користувачів за ім'ям або email
        endpoint.MapGet("search", ([FromQuery] string query, FoodOrderContext context) =>
        {
            var users = context.Users
                .Where(u => u.Name.Contains(query) || u.Email.Contains(query))
                .ToList();

            return Results.Ok(users);
        });

        // Додати користувача
        endpoint.MapPost("/", async ([FromBody] User user, FoodOrderContext context) =>
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Results.Created("", user);
        });

        // Оновити користувача
        endpoint.MapPut("/", (User user, FoodOrderContext context) =>
        {
            var oldUser = context.Users.Find(user.Id);
            if (oldUser != null)
            {
                oldUser.Name = user.Name;
                oldUser.Email = user.Email;
                oldUser.Password = user.Password;
                context.SaveChanges();
                return Results.Ok(user);
            }
            return Results.NotFound();
        });

        // Видалити користувача
        endpoint.MapDelete("{id}", (int id, FoodOrderContext context) =>
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
                return Results.NoContent();
            }
            return Results.NotFound();
        });

        return app;
    }
}