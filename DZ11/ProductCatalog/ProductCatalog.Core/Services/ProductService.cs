using ProductCatalog.Core.Interfaces;
using ProductCatalog.Entities.Models;
using ProductCatalog.Storage;

namespace ProductCatalog.Core.Services;

public class ProductService : IProductService
{
    private readonly ProductContext _context;

    public ProductService(ProductContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public Product AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }

    public Product UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
        return product;
    }

    public void DeleteProduct(int id)
    {
        var product = GetProductById(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    public void DeleteAllProducts()
    {
        var allProducts = _context.Products.ToList();
        foreach (var product in allProducts)
        {
            _context.Products.Remove(product);
        }
        _context.SaveChanges();
    }
}
