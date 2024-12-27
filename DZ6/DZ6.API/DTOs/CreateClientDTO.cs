namespace DZ6.API.DTOs;

public class CreateClientDTO
{
    public string FirstName { get; set; } // Ім'я клієнта
    public string LastName { get; set; }  // Прізвище клієнта
    public DateTime DateOfBirth { get; set; } // Дата народження клієнта
}
