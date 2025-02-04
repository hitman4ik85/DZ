namespace PeopleBudgetTracker.Core.DTOs;

public class TransactionDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int CategoryId { get; set; }
    public int AccountId { get; set; }
}
