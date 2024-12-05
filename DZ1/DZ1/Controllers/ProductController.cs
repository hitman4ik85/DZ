using DZ1.Models;
using Microsoft.AspNetCore.Mvc;

namespace DZ1.Controllers;

public class ProductController : Controller
{
    public IActionResult ProductsList()
    {
        // Приклад для виводу на сторінку через Model
        var category = new Category
        {
            Id = 1,
            Name = "Електроніка",
            Description = "Смартфони, ноутбуки, аксесуари та інші пристрої, які полегшують вам життя",
            Discount = 10
        };

        var product = new Product
        {
            Id = 1,
            Name = "Ноутбук",
            Price = 1500.50,
            Description = "Сучасний ноутбук марки Dell, з 32 ГБ оперативної пам’яті",
            ManufactureDate = new DateTime(2024, 2, 13),
            Category = category
        };

        // Приклад для виводу через ViewBag, ViewData
        ViewBag.Description = product.Description; //для відображення опису товару через ViewBag
        ViewBag.CategoryDescription = product.Category.Description; //для відображення опису категорії через ViewBag
        ViewData["ManufactureDate"] = product.ManufactureDate.ToString("dd MMMM yyyy року", new System.Globalization.CultureInfo("uk-UA")); //відображення українською мовою через ViewData

        return View(product); //відкриває нашу сторінку з продукцією
    }
}
