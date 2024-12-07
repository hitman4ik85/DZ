using DZ2.Models;

namespace DZ2.Data;

// Реалізація для Employee
public class EmployeeDatabase : IDatabase<Employee>
{
    private readonly List<Employee> _employees = new(); // Симуляція бази даних

    public IEnumerable<Employee> Get()
    {
        return _employees;
    }

    public void Add(Employee item)
    {
        _employees.Add(item);
    }

    public void Update(Employee oldItem, Employee newItem)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == oldItem.Id);
        if (employee != null)
        {
            employee.Name = newItem.Name;
            employee.Surname = newItem.Surname;
            employee.DateOfBirth = newItem.DateOfBirth;
            employee.Gender = newItem.Gender;
            employee.Position = newItem.Position;
            employee.Salary = newItem.Salary;
        }
    }

    public void Remove(Employee item)
    {
        _employees.RemoveAll(e => e.Id == item.Id);
    }
}
