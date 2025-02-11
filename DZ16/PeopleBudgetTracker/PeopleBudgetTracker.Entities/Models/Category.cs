using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleBudgetTracker.Entities.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
