using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Entities.Models;
using ProductCatalog.API.DTOs;

namespace ProductCatalog.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = _productService.GetProducts()
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });

        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductDTO> GetProductById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound($"Product with ID {id} not found.");

        var productDTO = new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };

        return Ok(productDTO);
    }

    [HttpPost]
    public ActionResult<ProductDTO> AddProduct([FromBody] AddProductRequestDTO request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock
        };

        var createdProduct = _productService.AddProduct(product);

        var productDTO = new ProductDTO
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
            Stock = createdProduct.Stock
        };

        return CreatedAtAction(nameof(GetProductById), new { id = productDTO.Id }, productDTO);
    }

    [HttpPut("{id}")]
    public ActionResult<ProductDTO> UpdateProduct(int id, [FromBody] AddProductRequestDTO request)
    {
        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null)
            return NotFound($"Product with ID {id} not found.");

        existingProduct.Name = request.Name;
        existingProduct.Description = request.Description;
        existingProduct.Price = request.Price;
        existingProduct.Stock = request.Stock;

        var updatedProduct = _productService.UpdateProduct(existingProduct);

        var productDTO = new ProductDTO
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            Stock = updatedProduct.Stock
        };

        return Ok(productDTO);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound($"Product with ID {id} not found.");

        _productService.DeleteProduct(id);
        return Ok($"Product with ID {id} deleted successfully.");
    }

    [HttpDelete]
    public IActionResult BulkDeleteProducts()
    {
        _productService.DeleteAllProducts();
        return Ok("All products deleted successfully.");
    }
}
