namespace PeopleBudgetTracker.Core.DTOs;

public class CategoryStatisticsDTO
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int OperationCount { get; set; }
    public decimal TotalAmount { get; set; }
}
