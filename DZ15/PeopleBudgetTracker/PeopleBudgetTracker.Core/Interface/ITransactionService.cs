using PeopleBudgetTracker.Core.DTOs;

namespace PeopleBudgetTracker.Core.Interfaces;

public interface ITransactionService
{
    Task<TransactionDTO> AddExpenseAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default);
    Task<TransactionDTO> AddIncomeAsync(TransactionDTO transactionDto, CancellationToken cancellationToken = default);
    Task<AccountDTO> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken = default);
    Task<IEnumerable<TransactionDTO>> GetTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
