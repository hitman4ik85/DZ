using DZ6.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetProducts(string? filter, int skip, int take); //метод отримання продуктів
    Product GetProductById(int id); //отримати продукт по його id

    Product AddProduct(Product product); //добавити продукт
    Product UpdateProduct(Product product); //оновлення продукту
    void DeleteProduct(int id); //видалення продукту
}
