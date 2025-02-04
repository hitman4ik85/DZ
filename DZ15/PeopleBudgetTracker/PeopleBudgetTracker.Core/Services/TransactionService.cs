using Microsoft.EntityFrameworkCore;
using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Interfaces;
using PeopleBudgetTracker.Entities.Models;
using PeopleBudgetTracker.Storage;

namespace PeopleBudgetTracker.Core.Services;

public class TransactionService : ITransactionService
{
    private readonly PeopleBudgetTrackerContext _context;

    public TransactionService(PeopleBudgetTrackerContext context)
    {
        _context = context;
    }

    public async Task<TransactionDTO> AddExpenseAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FindAsync(transactionDto.AccountId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("Account not found", nameof(transactionDto.AccountId));
        }

        account.Balance -= transactionDto.Amount;

        var expense = new ExpenseOperation
        {
            Amount = transactionDto.Amount,
            Description = transactionDto.Description,
            CreatedAt = transactionDto.Date,
            AccountId = transactionDto.AccountId,
            CategoryId = transactionDto.CategoryId,
            SourceOfExpense = "Manual Entry"
        };

        _context.ExpenseOperations.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        return transactionDto;
    }

    public async Task<TransactionDTO> AddIncomeAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FindAsync(transactionDto.AccountId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("Account not found", nameof(transactionDto.AccountId));
        }

        account.Balance += transactionDto.Amount;

        var income = new IncomeOperation
        {
            Amount = transactionDto.Amount,
            Description = transactionDto.Description,
            CreatedAt = transactionDto.Date,
            AccountId = transactionDto.AccountId,
            CategoryId = transactionDto.CategoryId
        };

        _context.IncomeOperations.Add(income);
        await _context.SaveChangesAsync(cancellationToken);

        return transactionDto;
    }

    public async Task<AccountDTO> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("User account not found", nameof(userId));
        }

        account.Balance = newBalance;
        await _context.SaveChangesAsync(cancellationToken);

        return new AccountDTO { Id = account.Id, Balance = account.Balance };
    }

    public async Task<IEnumerable<TransactionDTO>> GetTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("User account not found", nameof(userId));
        }

        var transactions = await _context.IncomeOperations
            .Where(t => t.AccountId == account.Id)
            .Select(t => new TransactionDTO
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.CreatedAt,
                CategoryId = t.CategoryId,
                AccountId = t.AccountId
            })
            .ToListAsync(cancellationToken);

        return transactions;
    }
}
