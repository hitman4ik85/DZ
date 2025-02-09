using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;

namespace CountryCityApi.Endpoints;

public static class HotelEndpoints
{
    public static WebApplication AddHotelEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("api/hotels/");

        // готель за ID
        endpoint.MapGet("{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var hotel = context.Hotels.Find(id);
            if (hotel == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(hotel);
        });

        // додати готель до міста
        endpoint.MapPost("city/{id}/", ([FromRoute] int id, [FromBody] Hotel hotel, CountryCityContext context) =>
        {
            var city = context.Cities.Find(id);
            if (city == null)
            {
                return Results.NotFound();
            }

            hotel.CityId = id;
            context.Hotels.Add(hotel);
            context.SaveChanges();

            return Results.Created("", hotel);
        });

        // оновити готель
        endpoint.MapPut("/", ([FromBody] Hotel hotel, CountryCityContext context) =>
        {
            var oldHotel = context.Hotels.Find(hotel.Id);
            if (oldHotel == null)
            {
                return Results.NotFound();
            }

            oldHotel.Name = hotel.Name;
            oldHotel.Stars = hotel.Stars;
            oldHotel.PriceForOneNight = hotel.PriceForOneNight;
            context.SaveChanges();

            return Results.Ok(hotel);
        });

        // видалити готель
        endpoint.MapDelete("{id}", ([FromRoute] int id, CountryCityContext context) =>
        {
            var hotel = context.Hotels.Find(id);
            if (hotel == null)
            {
                return Results.NotFound();
            }

            context.Hotels.Remove(hotel);
            context.SaveChanges();

            return Results.NoContent();
        });

        return app;
    }
}
