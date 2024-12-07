using DZ2.Data;
using DZ2.Models;
using Microsoft.AspNetCore.Mvc;

namespace DZ2.Controllers;

public class ManagerController : Controller
{
    private static readonly IDatabase<Manager> _managerDatabase = new ManagerDatabase();

    static ManagerController()
    {
        // для прикллада менеджерів
        _managerDatabase.Add(new Manager
        {
            Id = 1,
            Name = "Сергій",
            Surname = "Сергійович",
            DateOfBirth = new DateTime(1983, 3, 13),
            Gender = "Чоловік",
            Position = "Старший менеджер",
            Salary = 40000,
            EmployeeCount = EmployeeController._employeeDatabase.Get().Count()
        });
    }

    [HttpGet]
    public IActionResult ManagersList()
    {
        return View(_managerDatabase.Get());
    }

    [HttpGet]
    public IActionResult AddManager()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddManager(Manager manager)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        manager.Id = _managerDatabase.Get().Count() + 1;
        manager.EmployeeCount = EmployeeController._employeeDatabase.Get().Count();
        _managerDatabase.Add(manager);

        return RedirectToAction(nameof(ManagersList));
    }
}
