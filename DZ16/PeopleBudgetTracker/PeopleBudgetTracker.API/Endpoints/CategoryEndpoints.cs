using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PeopleBudgetTracker.API.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication MapCategoryEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/categories");

        endpoints.MapPost("/", [Authorize] async (CategoryDTO categoryDto, int userId, ICategoryService service) =>
        {
            return Results.Ok(await service.AddCategoryAsync(categoryDto, userId));
        });

        return app;
    }
}
