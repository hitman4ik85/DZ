using FoodPlacesApp.Data;
using FoodPlacesApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodPlacesApp.Controllers;

public class FoodPlaceController : Controller
{
    private static readonly IDatabase<FoodPlace> _foodPlaceDatabase = new FoodPlaceDatabase();

    static FoodPlaceController()
    {
        _foodPlaceDatabase.Add(new FoodPlace { Id = 1, Name = "Pizza House", Address = "Sumska str. 31", AveragePrice = 20000.5 });
        _foodPlaceDatabase.Add(new FoodPlace { Id = 2, Name = "Burger King", Address = "Harkivska str. 45", AveragePrice = 15000 });
        _foodPlaceDatabase.Add(new FoodPlace { Id = 3, Name = "Sushi World", Address = "Kirova str. 56", AveragePrice = 30000 });
    }

    [HttpGet]
    public IActionResult GetFoodPlaces()
    {
        var foodPlaces = _foodPlaceDatabase.Get();
        return View(foodPlaces);
    }

    [HttpGet]
    public IActionResult AddFoodPlace()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddFoodPlace(FoodPlace foodPlace)
    {
        _foodPlaceDatabase.Add(foodPlace);

        return RedirectToAction(nameof(GetFoodPlaces));
    }

    [HttpGet]
    public IActionResult DeleteFoodPlace(int id)
    {
        var foodPlace = _foodPlaceDatabase.Get().First(fp => fp.Id == id);
        return View(foodPlace);
    }

    [HttpPost]
    public IActionResult DeleteFoodPlace(FoodPlace foodPlace)
    {
        _foodPlaceDatabase.Remove(foodPlace);

        return RedirectToAction(nameof(GetFoodPlaces));
    }

    [HttpGet]
    public IActionResult EditFoodPlace(int id)
    {
        return View(_foodPlaceDatabase.Get().First(fp => fp.Id == id));
    }

    [HttpPost]
    public IActionResult EditFoodPlace(FoodPlace foodPlace)
    {
        var oldFoodPlace = _foodPlaceDatabase.Get().First(fp => fp.Id == foodPlace.Id);
        _foodPlaceDatabase.Update(oldFoodPlace, foodPlace);
        return RedirectToAction(nameof(GetFoodPlaces));
    }
}
