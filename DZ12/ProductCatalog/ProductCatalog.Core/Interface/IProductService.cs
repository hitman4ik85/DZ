using ProductCatalog.API.DTOs;

namespace ProductCatalog.Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ProductDTO> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductDTO> AddProductAsync(AddProductRequestDTO productDto, CancellationToken cancellationToken = default);
    Task<ProductDTO> UpdateProductAsync(int id, AddProductRequestDTO productDto, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteAllProductsAsync(CancellationToken cancellationToken = default);
}
