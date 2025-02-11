using PeopleBudgetTracker.Core.DTOs;

namespace PeopleBudgetTracker.Core.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDTO>> GetTransactionsByUserIdAsync(int userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
    Task<TransactionDTO> AddExpenseAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default);
    Task<TransactionDTO> AddIncomeAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default);
    Task<TransactionDTO> UpdateTransactionAsync(int userId, TransactionDTO transactionDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTransactionAsync(int userId, int transactionId, CancellationToken cancellationToken = default);
    Task<AccountDTO> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalIncomeAsync(int userId, DateTime date, string currency, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalExpensesAsync(int userId, DateTime date, string currency, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryStatisticsDTO>> GetCategoryStatisticsAsync(int userId, DateTime month, CancellationToken cancellationToken = default);
}
