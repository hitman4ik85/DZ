namespace DZ3.Models;

public class Book : Product
{
    public Author Author { get; set; } = new();
}