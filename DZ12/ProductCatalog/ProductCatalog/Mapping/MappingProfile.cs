using AutoMapper;
using ProductCatalog.API.DTOs;
using ProductCatalog.Entities.Models;

namespace ProductCatalog.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddProductRequestDTO, Product>();
        CreateMap<Product, ProductDTO>();
    }
}
