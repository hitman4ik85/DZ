using DZ6.API.DTOs;
using DZ6.Core.Interfaces;
using DZ6.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DZ6.API.Controllers;

[ApiController] //додаємо атрибут
[Route("api/v1/products")] //додаємо атрибут, щоб було зрозуміло що це(вказуєм шлях)(v1-версія APIшки)
public class ProductController : ControllerBase //наслідуємо від ControllerBase, і нам потрібен тепер сервіс з яким ми раніше працювали
{
    private readonly IProductService _productService; //в нас все має бути через абстракцію, тому ми звертаємось до інтерфейсу IProductService

    public ProductController(IProductService productService)
    {
        _productService = productService; //створюєм конструктор, для того щоб підключити ДепенденсІнжекшен
    }


    [HttpGet]			//тому що повертати нам потрібно ProductDTO
    public ActionResult<IEnumerable<ProductDTO>> GetProducts([FromQuery] string? filter, [FromQuery] int skip, [FromQuery] int take)
    {
        var products = _productService.GetProducts(filter, skip, take)
            .Select(p => new ProductDTO() //робимо селект з допомогою якого ми зможемо зробити перетворення через DTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Count = p.ProductStorageInfo.Count
            });

        return Ok(products);
    }

    [HttpPost]
    public ActionResult<ProductDTO> AddProduct([FromBody] CreateProductDTO createProductDTO)
    {
        try
        {
            var product = new Product()
            {
                Name = createProductDTO.Name,
                Price = createProductDTO.Price,
                Description = createProductDTO.Description,
                ProductStorageInfo = new ProductStorageInfo()
                {
                    Count = createProductDTO.Count
                }
            };
            var productFromDb = _productService.AddProduct(product);

            var productDTO = new ProductDTO()
            {
                Id = productFromDb.Id,
                Name = productFromDb.Name,
                Price = productFromDb.Price,
                Description = productFromDb.Description,
                Count = productFromDb.ProductStorageInfo.Count
            };
            return Created($"api/v1/products/{productFromDb.Id}", productDTO); // 201
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
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
            Price = product.Price,
            Description = product.Description,
            Count = product.ProductStorageInfo.Count
        };

        return Ok(productDTO);
    }

    [HttpPut("{id}")]
    public ActionResult<ProductDTO> UpdateProduct(int id, [FromBody] CreateProductDTO updateProductDTO)
    {
        try
        {
            var existingProduct = _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with id {id} not found.");
            }

            existingProduct.Name = updateProductDTO.Name;
            existingProduct.Price = updateProductDTO.Price;
            existingProduct.Description = updateProductDTO.Description;
            existingProduct.ProductStorageInfo.Count = updateProductDTO.Count;

            var updatedProduct = _productService.UpdateProduct(existingProduct);

            var productDTO = new ProductDTO()
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                Description = updatedProduct.Description,
                Count = updatedProduct.ProductStorageInfo.Count
            };

            return Ok(productDTO);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}