namespace DZ6.API.DTOs;

public class CreateProductDTO //такий вигляд буде бачити користувач
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Count { get; set; }
}
