using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs.Inputs;
using MovieAPI.DTOs.Outputs;
using MovieAPI.Services;

namespace MovieAPI.Controllers;

[ApiController]
[Route("api/films")]
public class FilmController : ControllerBase
{
    private readonly IFilmService _filmService;

    public FilmController(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [HttpGet] // api/films?skip=0&take=10
    public async Task<ActionResult<IEnumerable<FilmDTO>>> GetFilmsAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var films = await _filmService.GetFilmsAsync(skip, take);
        return Ok(films);
    }

    [HttpGet("{id}")] // api/films/{id}
    public async Task<ActionResult<FilmDTO>> GetFilmByIdAsync([FromRoute] int id)
    {
        var film = await _filmService.GetFilmByIdAsync(id);
        return Ok(film);
    }

    [HttpGet("search")] // api/films/search?title=example
    public async Task<ActionResult<IEnumerable<FilmDTO>>> SearchFilmsAsync([FromQuery] string title)
    {
        var films = await _filmService.SearchFilmsAsync(title);
        return Ok(films);
    }

    [HttpPost] // api/films
    public async Task<ActionResult<FilmDTO>> AddFilmAsync([FromBody] CreateFilmDTO createFilmDto)
    {
        var film = await _filmService.AddFilmAsync(createFilmDto);
        return Created($"api/films/{film.Id}", film);
    }
}
