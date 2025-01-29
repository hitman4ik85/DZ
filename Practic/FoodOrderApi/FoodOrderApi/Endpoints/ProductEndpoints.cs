using Microsoft.AspNetCore.Mvc;
using FoodOrderApi.Data;
using FoodOrderApi.Models;

namespace FoodOrderApi.Endpoints;

public static class ProductEndpoints
{
    public static WebApplication AddProductEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/products/");

        endpoint.MapGet("/", (FoodOrderContext context) =>
        {
            return Results.Ok(context.Products.ToList());
        });

        endpoint.MapGet("{id}", ([FromRoute] int id, FoodOrderContext context) =>
        {
            var product = context.Products.Find(id);
            return product != null ? Results.Ok(product) : Results.NotFound();
        });

        endpoint.MapPost("/", async ([FromBody] Product product, FoodOrderContext context) =>
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return Results.Created("", product);
        });

        endpoint.MapPut("/", (Product product, FoodOrderContext context) =>
        {
            var oldProduct = context.Products.Find(product.Id);
            if (oldProduct != null)
            {
                oldProduct.Name = product.Name;
                oldProduct.Description = product.Description;
                oldProduct.Price = product.Price;
                context.SaveChanges();
                return Results.Ok(product);
            }
            return Results.NotFound();
        });

        endpoint.MapDelete("{id}", (int id, FoodOrderContext context) =>
        {
            var product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
                return Results.NoContent();
            }
            return Results.NotFound();
        });

        return app;
    }
}