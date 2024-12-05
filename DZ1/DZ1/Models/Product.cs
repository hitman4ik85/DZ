namespace DZ1.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; } //назва продукту
    public double Price { get; set; } //в дробовому вигляду (30.50)
    public string? Description { get; set; } //опис товару
    public DateTime ManufactureDate { get; set; } //дата виготовлення товару
    public Category Category { get; set; } // Додано для зв’язку продукт з категорією
}