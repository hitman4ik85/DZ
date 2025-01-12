using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs.Inputs;
using MovieAPI.DTOs.Outputs;
using MovieAPI.Models;

namespace MovieAPI.Services;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
    Task<AuthorDTO> GetAuthorByIdAsync(int id);
    Task<AuthorDTO> AddAuthorAsync(CreateAuthorDTO createAuthorDto);
    Task DeleteAuthorAsync(int id);
}

public class AuthorService : IAuthorService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AuthorService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorDTO>> GetAuthorsAsync()
    {
        return await _context.Authors
            .Select(a => _mapper.Map<AuthorDTO>(a))
            .ToArrayAsync();
    }

    public async Task<AuthorDTO> GetAuthorByIdAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null) throw new KeyNotFoundException($"Author with ID {id} not found.");

        return _mapper.Map<AuthorDTO>(author);
    }

    public async Task<AuthorDTO> AddAuthorAsync(CreateAuthorDTO createAuthorDto)
    {
        var author = _mapper.Map<Author>(createAuthorDto);
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return _mapper.Map<AuthorDTO>(author);
    }

    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null) throw new KeyNotFoundException($"Author with ID {id} not found.");

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}
