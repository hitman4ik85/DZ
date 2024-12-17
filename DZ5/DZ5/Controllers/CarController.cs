using Lesson5WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lesson5WebApi.Controllers;

[ApiController]  // обовязковий атрибут
[Route("cars")]  // обовязковий атрибут
public class CarController : ControllerBase
{
    private static List<Car> cars = new List<Car>()
    {
        new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2021, Color = "Black", Price = 25000 },
        new Car { Id = 2, Brand = "Honda", Model = "Civic", Year = 2020, Color = "White", Price = 22000 },
        new Car { Id = 3, Brand = "BMW", Model = "X5", Year = 2019, Color = "Blue", Price = 45000 },
        new Car { Id = 4, Brand = "Ford", Model = "Focus", Year = 2018, Color = "Red", Price = 18000 },
        new Car { Id = 5, Brand = "Toyota", Model = "Corola", Year = 2022, Color = "Blue", Price = 28000 },
        new Car { Id = 6, Brand = "Mercedes", Model = "E-Class", Year = 2022, Color = "Silver", Price = 55000 },
        new Car { Id = 7, Brand = "Honda", Model = "C-RV", Year = 2022, Color = "White", Price = 32000 },
        new Car { Id = 8, Brand = "Tesla", Model = "Model-S", Year = 2023, Color = "Black", Price = 62000 },
        new Car { Id = 9, Brand = "Audi", Model = "A5", Year = 2018, Color = "Gray", Price = 28000 }

    };

    // 1. Отримати всі машини з пагінацією:
    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetCarsPaged([FromQuery] int skip, [FromQuery] int take)
    {
        var pagedCars = cars
            .Skip(skip) //методи LINQ
            .Take(take);

        return Ok(pagedCars);
    }

    // 2. Отримати машину по Id:
    [HttpGet("{id}")]
    public ActionResult<Car> GetCarById([FromRoute] int id)
    {                    //метод LINQ
        var car = cars.FirstOrDefault(c => c.Id == id);
        if (car == null) // перевіряємо чи є така машина
            return NotFound(); //якщо немає, повертаємо NotFound

        return Ok(car);
    }

    // 3. Отримати усі машини по бренду:
    [HttpGet("brand/{brand}")]
    public ActionResult<IEnumerable<Car>> GetCarsByBrand([FromRoute] string brand)
    {                   //метод LINQ
        var result = cars.Where(c => c.Brand == brand); // за умовою де бренд відповідає бренду

        return Ok(result);
    }

    // 4. Додати машину:
    [HttpPost]
    public ActionResult<Car> AddCar([FromBody] Car car)
    {               //метод LINQ
        car.Id = cars.Max(c => c.Id) + 1; // знаходимо максимальне id у базі за умовою, та додаємо +1 та надаємо цей id машині
        cars.Add(car);  // записуємо машину до бази

        return Created($"/cars/{car.Id}", car);
    }

    // 5. Видалити машину по Id:
    [HttpDelete("{id}")]
    public ActionResult<int> DeleteCarById([FromRoute] int id)
    {                   //метод LINQ
        var car = cars.FirstOrDefault(c => c.Id == id);
        if (car == null) 
            return NotFound();

        cars.Remove(car);

        return Ok(id);
    }

    // 6. Оновити машину:
    [HttpPut]
    public ActionResult<Car> UpdateCar([FromBody] Car upCar)
    {                   //метод LINQ
        var car = cars.FirstOrDefault(c => c.Id == upCar.Id);
        if (car == null) 
            return NotFound();

        car.Brand = upCar.Brand;
        car.Model = upCar.Model;
        car.Year = upCar.Year;
        car.Color = upCar.Color;
        car.Price = upCar.Price;

        return Ok(car);
    }

    // 7. Оновити колір машини:
    [HttpPatch("{id}/color")]
    public ActionResult<Car> UpdateCarColor([FromRoute] int id, [FromBody] string color)
    {                   //метод LINQ
        var car = cars.FirstOrDefault(c => c.Id == id);
        if (car == null) 
            return NotFound();

        car.Color = color;

        return Ok(car);
    }

    // 8. Видалити декілька машин по їх Id:
    [HttpDelete("delete-many")]
    public ActionResult<IEnumerable<int>> DeleteManyCars([FromBody] List<int> carId)
    {
        var delById = new List<int>(); // список видалених

        foreach (var id in carId)
        {                   //метод LINQ
            var car = cars.FirstOrDefault(c => c.Id == id); // пошук за умовою
            if (car != null)
            {
                cars.Remove(car); // видаляємо машину з бази(cars)
                delById.Add(id); // додамо Id до списку видалених
            }
        }

        return Ok(delById);
    }

    // 9. Отримати всі бренди машин (тільки унікальні):
    [HttpGet("/brands")]
    public ActionResult<IEnumerable<string>> GetUniqueBrands()
    {
        var brands = cars   //методи LINQ
            .Select(c => c.Brand) // дістаємо властивіть кожного нашого авто
            .Distinct(); // для уникнення однакових

        return Ok(brands);
    }
}
