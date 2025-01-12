namespace MovieAPI.DTOs.Inputs;

public class CreateFilmDTO
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AuthorId { get; set; }
}
