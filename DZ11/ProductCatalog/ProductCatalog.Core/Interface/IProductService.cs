using ProductCatalog.Entities.Models;

namespace ProductCatalog.Core.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetProducts();
    Product GetProductById(int id);
    Product AddProduct(Product product);
    Product UpdateProduct(Product product);
    void DeleteProduct(int id);
    void DeleteAllProducts();
}
