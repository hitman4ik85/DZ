namespace MovieAPI.DTOs.Outputs;

public class FilmDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuthorDTO Author { get; set; }
}
