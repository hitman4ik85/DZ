using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;
using CountryCityApi.Services;

namespace CountryCityApi.Endpoints;

public static class CountryEndpoints
{
    public static WebApplication AddCountryEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/country/");

        // всі країни
        endpoint.MapGet("/", (CountryCityContext context) =>
        {
            var countries = context.Countries.ToList();

            if (countries.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(countries);
        });

        // країна за ID
        endpoint.MapGet("{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var country = context.Countries.Find(id);

            if (country == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(country);
        });

        // країна за ім'ям
        endpoint.MapGet("name/{name}", ([FromRoute] string name, CountryCityContext context) =>
        {
            var country = context.Countries.FirstOrDefault(c => c.Name == name);

            if (country == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(country);
        });

        // отримати всі міста країни за їх ID
        endpoint.MapGet("{id}/city", ([FromRoute] int id, CountryCityContext context) =>
        {
            var cities = context.Cities.Where(c => c.CountryId == id).ToList();

            if (cities.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(cities);
        });

        // додати країну
        endpoint.MapPost("/", async ([FromBody] Country country, CountryCityContext context, IEmailSender emailSender) =>
        {
            context.Countries.Add(country);
            await context.SaveChangesAsync();

            await emailSender.SendAsync("Country Added", $"New country added: {country.Name}", CancellationToken.None);
            return Results.Created("", country);
        });

        // додати місто до країни
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

        // оновити країну
        endpoint.MapPut("/", (Country country, CountryCityContext context, IEmailSender emailSender) =>
        {
            var oldCountry = context.Countries.Find(country.Id);

            if (oldCountry == null)
            {
                return Results.NotFound();
            }

            oldCountry.Name = country.Name;
            oldCountry.Population = country.Population;
            oldCountry.MonetaryUnit = country.MonetaryUnit;
            oldCountry.Iso = country.Iso;
            context.SaveChanges();

            emailSender.Send("Country Updated", $"Updated country: {country.Name}");
            return Results.Ok(country);
        });

        // видалити країну разом з усім
        endpoint.MapDelete("{id}", (int id, CountryCityContext context, IEmailSender emailSender) =>
        {
            var country = context.Countries.Find(id);

            if (country == null)
            {
                return Results.NotFound();
            }

            var cities = context.Cities.Where(c => c.CountryId == id).ToList();
            var cityIds = cities.Select(c => c.Id).ToList();
            var hotels = context.Hotels.Where(h => cityIds.Contains(h.CityId)).ToList();

            context.Hotels.RemoveRange(hotels);
            context.Cities.RemoveRange(cities);
            context.Countries.Remove(country);
            context.SaveChanges();

            emailSender.Send("Country Deleted", $"Deleted country: {country.Name}");
            return Results.NoContent();
        });

        // отримати країни з сортуванням за параметром (name, population)
        endpoint.MapGet("sort/{sorting}", ([FromRoute] string sorting, CountryCityContext context) =>
        {
            IQueryable<Country> sortedCountries = sorting.ToLower() switch
            {
                "name" => context.Countries.OrderBy(c => c.Name),
                "population" => context.Countries.OrderByDescending(c => c.Population),
                _ => context.Countries
            };

            return Results.Ok(sortedCountries.ToList());
        });

        // cтолиця країни
        endpoint.MapGet("capital/{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var capital = context.Cities.FirstOrDefault(c => c.CountryId == id && c.IsCapitalCity);

            if (capital == null)
            {
                return Results.NotFound("No capital city found for this country.");
            }

            return Results.Ok(capital);
        });

        return app;
    }
}