using System.Security.Principal;

namespace PeopleBudgetTracker.Entities.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }

    public DateTime CreatedAt { get; set; }
}
