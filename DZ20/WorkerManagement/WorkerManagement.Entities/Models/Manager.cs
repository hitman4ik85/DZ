using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerManagement.Entities.Models;

public class Manager
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
