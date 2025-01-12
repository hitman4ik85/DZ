using AutoMapper;
using MovieAPI.DTOs.Inputs;
using MovieAPI.DTOs.Outputs;
using MovieAPI.Models;

namespace MovieAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateFilmDTO, Film>();
        CreateMap<Film, FilmDTO>();
        CreateMap<Author, AuthorDTO>();
        CreateMap<CreateAuthorDTO, Author>();
        CreateMap<Author, AuthorDTO>();
    }
}
