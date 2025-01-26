using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.DTOs;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Entities.Models;
using ProductCatalog.Storage;

namespace ProductCatalog.Core.Services;

public class ProductService : IProductService
{
    private readonly ProductContext _context;
    private readonly IMapper _mapper;

    public ProductService(ProductContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 0 || pageSize <= 0)
            throw new ArgumentException("Page and PageSize must be greater than 0.");

        var products = await _context.Products
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> AddProductAsync(AddProductRequestDTO request, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(request);
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> UpdateProductAsync(int id, AddProductRequestDTO request, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        _mapper.Map(request, product);
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAllProductsAsync(CancellationToken cancellationToken = default)
    {
        _context.Products.RemoveRange(_context.Products);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
