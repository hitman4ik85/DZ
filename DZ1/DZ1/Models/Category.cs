namespace DZ1.Models;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; } //ім'я категорії
    public string? Description { get; set; } //опис категорії
    public int Discount { get; set; } //знижка в цілих % (5, 10, 20, 30...)
}