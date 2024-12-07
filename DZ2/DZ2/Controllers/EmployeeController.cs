using DZ2.Data;
using DZ2.Models;
using Microsoft.AspNetCore.Mvc;

namespace DZ2.Controllers;

public class EmployeeController : Controller
{
    public static readonly IDatabase<Employee> _employeeDatabase = new EmployeeDatabase();

    static EmployeeController()
    {
        // для прикллада робітників
        _employeeDatabase.Add(new Employee
        {
            Id = 1,
            Name = "Андрій",
            Surname = "Андрійович",
            DateOfBirth = new DateTime(1985, 4, 26),
            Gender = "Чоловік",
            Position = "Інженер",
            Salary = 25000
        });
    }

    [HttpGet]
    public IActionResult EmployeesList()
    {
        return View(_employeeDatabase.Get());
    }

    [HttpGet]
    public IActionResult AddEmployee()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddEmployee(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        employee.Id = _employeeDatabase.Get().Count() + 1;
        _employeeDatabase.Add(employee);

        return RedirectToAction(nameof(EmployeesList));
    }
}
