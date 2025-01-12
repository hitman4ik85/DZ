using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs.Inputs;
using MovieAPI.DTOs.Outputs;
using MovieAPI.Models;

namespace MovieAPI.Services;

public interface IFilmService
{
    Task<IEnumerable<FilmDTO>> GetFilmsAsync(int skip, int take);
    Task<FilmDTO> GetFilmByIdAsync(int id);
    Task<IEnumerable<FilmDTO>> SearchFilmsAsync(string title);
    Task<FilmDTO> AddFilmAsync(CreateFilmDTO createFilmDto);
}

public class FilmService : IFilmService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public FilmService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FilmDTO>> GetFilmsAsync(int skip, int take)
    {
        return await _context.Films
            .Where(f => f.DeletedAt == null)
            .Include(f => f.Author)
            .Select(f => _mapper.Map<FilmDTO>(f))
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
    }

    public async Task<FilmDTO> GetFilmByIdAsync(int id)
    {
        var film = await _context.Films
            .Include(f => f.Author)
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt == null);

        if (film == null) throw new KeyNotFoundException($"Film with ID {id} not found.");

        return _mapper.Map<FilmDTO>(film);
    }

    public async Task<IEnumerable<FilmDTO>> SearchFilmsAsync(string title)
    {
        return await _context.Films
            .Where(f => f.Title.Contains(title) && f.DeletedAt == null)
            .Include(f => f.Author)
            .Select(f => _mapper.Map<FilmDTO>(f))
            .ToArrayAsync();
    }

    public async Task<FilmDTO> AddFilmAsync(CreateFilmDTO createFilmDto)
    {
        var author = await _context.Authors.FindAsync(createFilmDto.AuthorId);
        if (author == null) throw new ArgumentException($"Author with ID {createFilmDto.AuthorId} not found.");

        var film = _mapper.Map<Film>(createFilmDto);
        film.Author = author;
        film.CreatedAt = DateTime.UtcNow;

        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        return _mapper.Map<FilmDTO>(film);
    }
}
