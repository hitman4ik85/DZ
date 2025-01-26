using Microsoft.AspNetCore.Mvc;
using ProductCatalog.API.DTOs;
using ProductCatalog.Core.Interfaces;

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
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] int page = 0, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var products = await _productService.GetProductsAsync(page, pageSize, cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetProductByIdAsync(id, cancellationToken);
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] AddProductRequestDTO request, CancellationToken cancellationToken = default)
    {
        var createdProduct = await _productService.AddProductAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromBody] AddProductRequestDTO request, CancellationToken cancellationToken = default)
    {
        var updatedProduct = await _productService.UpdateProductAsync(id, request, cancellationToken);
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken = default)
    {
        await _productService.DeleteProductAsync(id, cancellationToken);
        return Ok($"Product with ID {id} deleted successfully.");
    }

    [HttpDelete]
    public async Task<IActionResult> BulkDeleteProducts(CancellationToken cancellationToken = default)
    {
        await _productService.DeleteAllProductsAsync(cancellationToken);
        return Ok("All products deleted successfully.");
    }
}
