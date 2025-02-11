namespace PeopleBudgetTracker.Core.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
}
