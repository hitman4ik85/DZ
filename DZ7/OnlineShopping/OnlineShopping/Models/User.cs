namespace OnlineShopping.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }

    public ICollection<Order> Orders { get; set; } //може мати декілька замовлень

    public DateTime CreatedAt { get; set; } //Дата та час, коли користувача було створено
    public DateTime ModifiedAt { get; set; } //Дата та час, коли змінювали користувача
    public DateTime? DeletedAt { get; set; } //Дата та час, для випадкового видалення, та змоги повернення,(видимість видалення - soft delete)
}