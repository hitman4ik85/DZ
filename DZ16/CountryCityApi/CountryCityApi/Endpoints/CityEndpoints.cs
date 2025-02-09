using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;

namespace CountryCityApi.Endpoints;

public static class CityEndpoints
{
    public static WebApplication AddCityEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/city/");

        // місто за ID
        endpoint.MapGet("{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var city = context.Cities.Find(id);

            if (city == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(city);
        });

        // оновити місто
        endpoint.MapPut("/", (City city, CountryCityContext context) =>
        {
            var oldCity = context.Cities.Find(city.Id);

            if (oldCity == null)
            {
                return Results.NotFound();
            }

            oldCity.Name = city.Name;
            oldCity.Population = city.Population;
            oldCity.IsCapitalCity = city.IsCapitalCity; // можливість змінювати столицю
            context.SaveChanges();

            return Results.Ok(city);
        });

        // видалити місто разом з готелями
        endpoint.MapDelete("{id}", (int id, CountryCityContext context) =>
        {
            var city = context.Cities.Find(id);

            if (city == null)
            {
                return Results.NotFound();
            }

            var hotels = context.Hotels.Where(h => h.CityId == id).ToList();
            context.Hotels.RemoveRange(hotels);
            context.Cities.Remove(city);
            context.SaveChanges();
            return Results.NoContent();
        });

        // всі готелі в місті
        endpoint.MapGet("{id}/hotels", ([FromRoute] int id, CountryCityContext context) =>
        {
            var hotels = context.Hotels.Where(h => h.CityId == id).ToList();

            if (!hotels.Any())
            {
                return Results.NotFound("No hotels found in this city.");
            }

            return Results.Ok(hotels);
        });

        // додати готель до міста
        endpoint.MapPost("{id}/hotels", async ([FromRoute] int id, [FromBody] Hotel hotel, CountryCityContext context) =>
        {
            var city = context.Cities.Find(id);

            if (city == null)
            {
                return Results.NotFound("City not found.");
            }

            hotel.CityId = id;
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();
            return Results.Created("", hotel);
        });

        return app;
    }
}
