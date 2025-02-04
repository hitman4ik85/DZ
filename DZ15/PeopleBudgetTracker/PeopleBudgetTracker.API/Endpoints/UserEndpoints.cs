using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Interfaces;

namespace PeopleBudgetTracker.API.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/users");

        // Реєстрація нового користувача
        endpoints.MapPost("/register", async (CreateUserDTO createUserDto, IUserService service) =>
        {
            await service.RegisterUserAsync(createUserDto);
            return Results.Ok();
        });

        // Логін користувача
        endpoints.MapPost("/login", async (string email, string password, IUserService service) =>
        {
            return Results.Ok(await service.LoginUserAsync(email, password));
        });

        // Оновлення даних користувача (без зміни пароля)
        endpoints.MapPut("/", async (UserDTO userDTO, IUserService service) =>
        {
            var user = await service.UpdateUserAsync(userDTO);
            return Results.Ok(user);
        });

        return app;
    }
}
