using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleBudgetTracker.Entities.Models;

public class Account
{
    public int Id { get; set; }
    public decimal Balance { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; }
}
