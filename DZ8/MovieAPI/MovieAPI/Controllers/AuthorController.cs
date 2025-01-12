using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs.Inputs;
using MovieAPI.DTOs.Outputs;
using MovieAPI.Services;

namespace MovieAPI.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet] // api/authors
    public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthorsAsync()
    {
        var authors = await _authorService.GetAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")] // api/authors/{id}
    public async Task<ActionResult<AuthorDTO>> GetAuthorByIdAsync([FromRoute] int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        return Ok(author);
    }

    [HttpPost] // api/authors
    public async Task<ActionResult<AuthorDTO>> AddAuthorAsync([FromBody] CreateAuthorDTO createAuthorDto)
    {
        var author = await _authorService.AddAuthorAsync(createAuthorDto);
        return Created($"api/authors/{author.Id}", author);
    }

    [HttpDelete("{id}")] // api/authors/{id}
    public async Task<ActionResult> DeleteAuthorAsync([FromRoute] int id)
    {
        await _authorService.DeleteAuthorAsync(id);
        return NoContent();
    }
}
