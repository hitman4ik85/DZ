using Microsoft.AspNetCore.Mvc;
using FoodOrderApi.Data;
using FoodOrderApi.Models;

namespace FoodOrderApi.Endpoints;

public static class OrderEndpoints
{
    public static WebApplication AddOrderEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/orders/");

        // Отримати всі замовлення
        endpoint.MapGet("/", (FoodOrderContext context) =>
        {
            return Results.Ok(context.Orders.ToList());
        });

        // Отримати замовлення за ID
        endpoint.MapGet("{id}", ([FromRoute] int id, FoodOrderContext context) =>
        {
            var order = context.Orders.Find(id);
            return order != null ? Results.Ok(order) : Results.NotFound();
        });

        // Пошук замовлень за ID користувача
        endpoint.MapGet("user/{userId}", ([FromRoute] int userId, FoodOrderContext context) =>
        {
            var orders = context.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            return Results.Ok(orders);
        });

        // Створити замовлення
        endpoint.MapPost("/", async ([FromBody] Order order, FoodOrderContext context) =>
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return Results.Created("", order);
        });

        return app;
    }
}