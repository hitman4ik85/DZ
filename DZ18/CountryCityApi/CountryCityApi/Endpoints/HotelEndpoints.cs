using Microsoft.AspNetCore.Mvc;
using CountryCityApi.Data;
using CountryCityApi.Models;
using CountryCityApi.Services;

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
        endpoint.MapPost("city/{id}/", async ([FromRoute] int id, [FromBody] Hotel hotel, CountryCityContext context, IEmailSender emailSender) =>
        {
            var city = context.Cities.Find(id);
            if (city == null)
            {
                return Results.NotFound();
            }

            hotel.CityId = id;
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            await emailSender.SendAsync("Hotel Added", $"New hotel added: {hotel.Name}", CancellationToken.None);
            return Results.Created("", hotel);
        });

        // оновити готель
        endpoint.MapPut("/", (Hotel hotel, CountryCityContext context, IEmailSender emailSender) =>
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

            emailSender.Send("Hotel Updated", $"Updated hotel: {hotel.Name}");
            return Results.Ok(hotel);
        });

        // видалити готель
        endpoint.MapDelete("{id}", (int id, CountryCityContext context, IEmailSender emailSender) =>
        {
            var hotel = context.Hotels.Find(id);
            if (hotel == null)
            {
                return Results.NotFound();
            }

            context.Hotels.Remove(hotel);
            context.SaveChanges();

            emailSender.Send("Hotel Deleted", $"Deleted hotel: {hotel.Name}");
            return Results.NoContent();
        });

        return app;
    }
}