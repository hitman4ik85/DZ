using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PeopleBudgetTracker.API.Endpoints;

public static class TransactionEndpoints
{
    public static WebApplication MapTransactionEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/transactions");

        // Додавання витрати (авторизований доступ)
        endpoints.MapPost("/expense", [Authorize] async (TransactionDTO transactionDto, ITransactionService service) =>
        {
            return Results.Ok(await service.AddExpenseAsync(transactionDto));
        });

        // Додавання доходу (авторизований доступ)
        endpoints.MapPost("/income", [Authorize] async (TransactionDTO transactionDto, ITransactionService service) =>
        {
            return Results.Ok(await service.AddIncomeAsync(transactionDto));
        });

        // Оновлення балансу (авторизований доступ)
        endpoints.MapPut("/balance/{userId}", [Authorize] async (int userId, decimal newBalance, ITransactionService service) =>
        {
            return Results.Ok(await service.UpdateBalanceAsync(userId, newBalance));
        });

        // Отримання транзакцій користувача (авторизований доступ)
        endpoints.MapGet("/{userId}", [Authorize] async (int userId, ITransactionService service) =>
        {
            return Results.Ok(await service.GetTransactionsByUserIdAsync(userId));
        });

        return app;
    }
}
