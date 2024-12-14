namespace DZ3.Models;

// Базовий клас для книжок
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Pages { get; set; }
}