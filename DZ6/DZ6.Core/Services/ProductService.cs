using DZ6.Core.Interfaces;
using DZ6.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Services;

public class ProductService : IProductService //імплементує наш інтерфейс IProductService (класи та інтерфейси потрібні для Депенденс Інжекшен)
{
    private readonly IRepository _repository; //тепер нам потрібен GenericRepository.cs, але бізнес логіка(OnlineShopProject.Core) не має ссилки на OnlineShopProject.Storage, 
                                              //але вона знає про інтерфейс(IRepository.cs), таким чином ми можемо використовувати будь який репозиторій, при налаштуванні через ДепенденсІнжекшен
    public ProductService(IRepository repository)
    {
        _repository = repository; //викликаємо(реалізуємо) його через конструктор
    }

    public IEnumerable<Product> GetProducts(string? filter, int skip, int take)
    {
        var productsQuery = _repository.GetAll<Product>() //нам потрібен наш репозиторій і з нього отримуємо всі продукти (запитами)
            .Include(p => p.ProductStorageInfo)
            .AsQueryable();

        if (filter != null)
        {			//відфільтруємо наші дані через метод where:
            productsQuery = productsQuery.Where(p => p.Name.Contains(filter) || p.Description.Contains(filter)); //якщо у нашого продукта ім'я містить те що написано у нашому фільтрі
                                                                                                                 //або якщо в продукта опис теж містить ось цей фільтр(слово яке ми передамо)
        }

        return productsQuery.OrderBy(p => p.Name) //робимо пагінацію на вивід даних
            .Skip(skip) //пропустити
            .Take(take) //взяти
            .ToArray(); //зведемо всі дані до масиву
    }

    public Product GetProductById(int id) //отримання продукта по id
    {
        return _repository.GetAll<Product>() //звертаємось до нашого репозиторія і отримуємо перший рподук за нашою умовою
            .FirstOrDefault(p => p.Id == id);
    }

    public Product AddProduct(Product product)
    {
        ValidateProduct(product); //робимо валідацію(Alt+Enter -> Create method)

        var productFromDb = _repository.Add(product); //звертаємось до нашого репозиторія та додаємо продукт
        _repository.SaveChanges(); //зберігаємо зміни

        return productFromDb; //повертаємо наш продукт
    }

    public Product UpdateProduct(Product product)
    {
        ValidateProduct(product);

        var productFromDb = _repository.Update(product);
        _repository.SaveChanges();

        return productFromDb;
    }

    public void DeleteProduct(int id)
    {
        var product = GetProductById(id); //отримуємо продукт через метод, і туди передаємо наш продукт
        _repository.Delete(product);
        _repository.SaveChanges();
    }

    private void ValidateProduct(Product product) //валідація та помилки, робиться в самому низу
    {
        if (string.IsNullOrEmpty(product.Name)) //чи наш продукт валідний, якщо щось не так ми викидуємо помилки
        {
            throw new ArgumentException("Name should not be empty", nameof(product.Name));
        }
        if (string.IsNullOrEmpty(product.Description))
        {
            throw new ArgumentException("Description should not be empty", nameof(product.Description));
        }
        if (product.Price < 0)
        {
            throw new ArgumentException("Price should not be less than o", nameof(product.Price));
        }
        if (product.ProductStorageInfo.Count < 0)
        {
            throw new ArgumentException("Count should not be less than o", nameof(product.ProductStorageInfo.Count));
        }
    }
}
