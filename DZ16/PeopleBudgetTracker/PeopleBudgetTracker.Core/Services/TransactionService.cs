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

    public async Task<IEnumerable<TransactionDTO>> GetTransactionsByUserIdAsync(int userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("User account not found", nameof(userId));
        }

        var incomeTransactions = await _context.IncomeOperations
            .Where(t => t.AccountId == account.Id &&
                        (!fromDate.HasValue || t.CreatedAt >= fromDate.Value) &&
                        (!toDate.HasValue || t.CreatedAt <= toDate.Value))
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

        var expenseTransactions = await _context.ExpenseOperations
            .Where(t => t.AccountId == account.Id &&
                        (!fromDate.HasValue || t.CreatedAt >= fromDate.Value) &&
                        (!toDate.HasValue || t.CreatedAt <= toDate.Value))
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

        return incomeTransactions.Concat(expenseTransactions);
    }

    public async Task<TransactionDTO> AddExpenseAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == transactionDto.AccountId, cancellationToken);
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
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == transactionDto.AccountId, cancellationToken);
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

    public async Task<TransactionDTO> UpdateTransactionAsync(int userId, TransactionDTO transactionDto, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("User account not found", nameof(userId));
        }

        var incomeTransaction = await _context.IncomeOperations.FindAsync(transactionDto.Id);
        var expenseTransaction = await _context.ExpenseOperations.FindAsync(transactionDto.Id);

        if (incomeTransaction != null && incomeTransaction.AccountId == account.Id)
        {
            incomeTransaction.Amount = transactionDto.Amount;
            incomeTransaction.Description = transactionDto.Description;
            incomeTransaction.CategoryId = transactionDto.CategoryId;
            incomeTransaction.CreatedAt = transactionDto.Date;
        }
        else if (expenseTransaction != null && expenseTransaction.AccountId == account.Id)
        {
            expenseTransaction.Amount = transactionDto.Amount;
            expenseTransaction.Description = transactionDto.Description;
            expenseTransaction.CategoryId = transactionDto.CategoryId;
            expenseTransaction.CreatedAt = transactionDto.Date;
        }
        else
        {
            throw new ArgumentException("Transaction not found", nameof(transactionDto.Id));
        }

        await _context.SaveChangesAsync(cancellationToken);
        return transactionDto;
    }

    public async Task<bool> DeleteTransactionAsync(int userId, int transactionId, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException("User account not found", nameof(userId));
        }

        var incomeTransaction = await _context.IncomeOperations.FindAsync(transactionId);
        var expenseTransaction = await _context.ExpenseOperations.FindAsync(transactionId);

        if (incomeTransaction != null && incomeTransaction.AccountId == account.Id)
        {
            _context.IncomeOperations.Remove(incomeTransaction);
        }
        else if (expenseTransaction != null && expenseTransaction.AccountId == account.Id)
        {
            _context.ExpenseOperations.Remove(expenseTransaction);
        }
        else
        {
            throw new ArgumentException("Transaction not found", nameof(transactionId));
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
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

    public async Task<decimal> GetTotalIncomeAsync(int userId, DateTime date, string currency, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
            throw new ArgumentException("User account not found", nameof(userId));

        return await _context.IncomeOperations
            .Where(t => t.AccountId == account.Id && t.Currency == currency && t.CreatedAt.Date == date.Date)
            .SumAsync(t => t.Amount, cancellationToken);
    }

    public async Task<decimal> GetTotalExpensesAsync(int userId, DateTime date, string currency, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
            throw new ArgumentException("User account not found", nameof(userId));

        return await _context.ExpenseOperations
            .Where(t => t.AccountId == account.Id && t.Currency == currency && t.CreatedAt.Date == date.Date)
            .SumAsync(t => t.Amount, cancellationToken);
    }

    public async Task<IEnumerable<CategoryStatisticsDTO>> GetCategoryStatisticsAsync(int userId, DateTime month, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
            throw new ArgumentException("User account not found", nameof(userId));

        var startDate = new DateTime(month.Year, month.Month, 1);
        var endDate = startDate.AddMonths(1);

        var incomeStats = await _context.IncomeOperations
            .Where(o => o.AccountId == account.Id && o.CreatedAt >= startDate && o.CreatedAt < endDate)
            .GroupBy(o => o.CategoryId)
            .Select(g => new CategoryStatisticsDTO
            {
                CategoryId = g.Key,
                CategoryName = g.First().Category.Name,
                OperationCount = g.Count(),
                TotalAmount = g.Sum(o => o.Amount)
            })
            .ToListAsync(cancellationToken);

        var expenseStats = await _context.ExpenseOperations
            .Where(o => o.AccountId == account.Id && o.CreatedAt >= startDate && o.CreatedAt < endDate)
            .GroupBy(o => o.CategoryId)
            .Select(g => new CategoryStatisticsDTO
            {
                CategoryId = g.Key,
                CategoryName = g.First().Category.Name,
                OperationCount = g.Count(),
                TotalAmount = -g.Sum(o => o.Amount) // Витрати робимо негативними
            })
            .ToListAsync(cancellationToken);

        return incomeStats.Concat(expenseStats);
    }
}
