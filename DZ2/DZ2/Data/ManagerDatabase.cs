using DZ2.Models;

namespace DZ2.Data;

// Реалізація для Manager
public class ManagerDatabase : IDatabase<Manager>
{
    private readonly List<Manager> _managers = new(); // Симуляція бази даних

    public IEnumerable<Manager> Get()
    {
        return _managers;
    }

    public void Add(Manager item)
    {
        _managers.Add(item);
    }

    public void Update(Manager oldItem, Manager newItem)
    {
        var manager = _managers.FirstOrDefault(m => m.Id == oldItem.Id);
        if (manager != null)
        {
            manager.Name = newItem.Name;
            manager.Surname = newItem.Surname;
            manager.DateOfBirth = newItem.DateOfBirth;
            manager.Gender = newItem.Gender;
            manager.Position = newItem.Position;
            manager.Salary = newItem.Salary;
            manager.EmployeeCount = newItem.EmployeeCount;
        }
    }

    public void Remove(Manager item)
    {
        _managers.RemoveAll(m => m.Id == item.Id);
    }
}
