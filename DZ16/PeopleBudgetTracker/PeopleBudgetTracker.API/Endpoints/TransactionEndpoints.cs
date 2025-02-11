using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PeopleBudgetTracker.API.Endpoints;

public static class TransactionEndpoints
{
    public static WebApplication MapTransactionEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/transactions");

        // Отримати операції з фільтром по даті
        endpoints.MapGet("/{userId}", [Authorize] async (int userId, DateTime? fromDate, DateTime? toDate, ITransactionService service) =>
        {
            return Results.Ok(await service.GetTransactionsByUserIdAsync(userId, fromDate, toDate));
        });

        // Додавання витрати
        endpoints.MapPost("/expense", [Authorize] async (TransactionDTO transactionDto, ITransactionService service) =>
        {
            return Results.Ok(await service.AddExpenseAsync(transactionDto));
        });

        // Додавання доходу
        endpoints.MapPost("/income", [Authorize] async (TransactionDTO transactionDto, ITransactionService service) =>
        {
            return Results.Ok(await service.AddIncomeAsync(transactionDto));
        });

        // Оновлення операції
        endpoints.MapPut("/{userId}", [Authorize] async (int userId, TransactionDTO transactionDto, ITransactionService service) =>
        {
            return Results.Ok(await service.UpdateTransactionAsync(userId, transactionDto));
        });

        // Видалення операції
        endpoints.MapDelete("/{userId}/{transactionId}", [Authorize] async (int userId, int transactionId, ITransactionService service) =>
        {
            return Results.Ok(await service.DeleteTransactionAsync(userId, transactionId));
        });

        endpoints.MapGet("/total-income/{userId}", [Authorize] async (int userId, DateTime date, string currency, ITransactionService service) =>
        {
            return Results.Ok(await service.GetTotalIncomeAsync(userId, date, currency));
        });

        endpoints.MapGet("/total-expenses/{userId}", [Authorize] async (int userId, DateTime date, string currency, ITransactionService service) =>
        {
            return Results.Ok(await service.GetTotalExpensesAsync(userId, date, currency));
        });
        //ендпоінт для отримання статистики по категоріях
        endpoints.MapGet("/statistics/{userId}", [Authorize] async (int userId, DateTime month, ITransactionService service) =>
        {
            return Results.Ok(await service.GetCategoryStatisticsAsync(userId, month));
        });

        return app;
    }
}
