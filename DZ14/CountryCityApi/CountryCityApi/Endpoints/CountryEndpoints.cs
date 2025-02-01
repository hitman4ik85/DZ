using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;

namespace CountryCityApi.Endpoints;

public static class CountryEndpoints
{
    public static WebApplication AddCountryEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/country/");

        endpoint.MapGet("/", (CountryCityContext context) =>
        {
            var countries = context.Countries.ToList();

            if (countries.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(countries);
        });

        endpoint.MapGet("{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var country = context.Countries.Find(id);

            if (country == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(country);
        });

        endpoint.MapGet("name/{name}", ([FromRoute] string name, CountryCityContext context) =>
        {
            var country = context.Countries.FirstOrDefault(c => c.Name == name);

            if (country == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(country);
        });

        endpoint.MapGet("{id}/city", ([FromRoute] int id, CountryCityContext context) =>
        {
            var cities = context.Cities.Where(c => c.CountryId == id).ToList();

            if (cities.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(cities);
        });

        endpoint.MapPost("/", async ([FromBody] Country country, CountryCityContext context) =>
        {
            context.Countries.Add(country);
            await context.SaveChangesAsync();
            return Results.Created("", country);
        });

        endpoint.MapPost("{id}/city", async ([FromRoute] int id, [FromBody] City city, CountryCityContext context) =>
        {
            var country = context.Countries.Find(id);

            if (country == null)
            {
                return Results.NotFound();
            }

            city.CountryId = id;
            context.Cities.Add(city);
            await context.SaveChangesAsync();
            return Results.Created("", city);
        });

        endpoint.MapPut("/", (Country country, CountryCityContext context) =>
        {
            var oldCountry = context.Countries.Find(country.Id);

            if (oldCountry == null)
            {
                return Results.NotFound();
            }

            oldCountry.Name = country.Name;
            oldCountry.Population = country.Population;
            oldCountry.MonetaryUnit = country.MonetaryUnit;
            context.SaveChanges();

            return Results.Ok(country);
        });

        endpoint.MapDelete("{id}", (int id, CountryCityContext context) =>
        {
            var country = context.Countries.Find(id);

            if (country == null)
            {
                return Results.NotFound();
            }

            var cities = context.Cities.Where(c => c.CountryId == id).ToList();
            context.Cities.RemoveRange(cities);
            context.Countries.Remove(country);
            context.SaveChanges();

            return Results.NoContent();
        });

        return app;
    }
}
