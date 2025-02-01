using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;

namespace CountryCityApi.Endpoints;

public static class CityEndpoints
{
    public static WebApplication AddCityEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/city/");

        // Отримати місто за ID
        endpoint.MapGet("{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var city = context.Cities.Find(id);

            if (city == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(city);
        });

        // Оновити місто
        endpoint.MapPut("/", (City city, CountryCityContext context) =>
        {
            var oldCity = context.Cities.Find(city.Id);

            if (oldCity == null)
            {
                return Results.NotFound();
            }

            oldCity.Name = city.Name;
            oldCity.Population = city.Population;
            context.SaveChanges();

            return Results.Ok(city);
        });

        // Видалити місто
        endpoint.MapDelete("{id}", (int id, CountryCityContext context) =>
        {
            var city = context.Cities.Find(id);

            if (city == null)
            {
                return Results.NotFound();
            }

            context.Cities.Remove(city);
            context.SaveChanges();
            return Results.NoContent();
        });

        return app;
    }
}
