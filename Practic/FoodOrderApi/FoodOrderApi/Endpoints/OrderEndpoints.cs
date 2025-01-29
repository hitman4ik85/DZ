using Microsoft.AspNetCore.Mvc;
using FoodOrderApi.Data;
using FoodOrderApi.Models;

namespace FoodOrderApi.Endpoints;

public static class OrderEndpoints
{
    public static WebApplication AddOrderEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/orders/");

        endpoint.MapPost("/", async ([FromBody] Order order, FoodOrderContext context) =>
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return Results.Created("", order);
        });

        endpoint.MapGet("/", (FoodOrderContext context) =>
        {
            return Results.Ok(context.Orders.ToList());
        });

        endpoint.MapGet("{id}", ([FromRoute] int id, FoodOrderContext context) =>
        {
            var order = context.Orders.Find(id);
            return order != null ? Results.Ok(order) : Results.NotFound();
        });

        return app;
    }
}