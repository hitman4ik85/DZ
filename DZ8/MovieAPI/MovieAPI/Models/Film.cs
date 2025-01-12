namespace MovieAPI.Models;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Author Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
