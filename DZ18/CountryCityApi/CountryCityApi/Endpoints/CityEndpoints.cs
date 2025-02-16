using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;
using CountryCityApi.Services;

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
        endpoint.MapPut("/", (City city, CountryCityContext context, IEmailSender emailSender) =>
        {
            var oldCity = context.Cities.Find(city.Id);

            if (oldCity == null)
            {
                return Results.NotFound();
            }

            oldCity.Name = city.Name;
            oldCity.Population = city.Population;
            oldCity.IsCapitalCity = city.IsCapitalCity;
            context.SaveChanges();

            emailSender.Send("City Updated", $"Updated city: {city.Name}");
            return Results.Ok(city);
        });

        // видалити місто разом з готелями
        endpoint.MapDelete("{id}", (int id, CountryCityContext context, IEmailSender emailSender) =>
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

            emailSender.Send("City Deleted", $"Deleted city: {city.Name}");
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

        // додати місто до країни
        endpoint.MapPost("{id}/", async ([FromRoute] int id, [FromBody] City city, CountryCityContext context, IEmailSender emailSender) =>
        {
            var country = context.Countries.Find(id);

            if (country == null)
            {
                return Results.NotFound();
            }

            city.CountryId = id;
            context.Cities.Add(city);
            await context.SaveChangesAsync();

            await emailSender.SendAsync("City Added", $"New city added: {city.Name}", CancellationToken.None);
            return Results.Created("", city);
        });

        return app;
    }
}