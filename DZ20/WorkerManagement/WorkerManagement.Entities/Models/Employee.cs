using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerManagement.Entities.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal HourlyRate { get; set; }

    [ForeignKey(nameof(Manager))]
    public int ManagerId { get; set; }
    public Manager Manager { get; set; }

    public ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
    public ICollection<Bonus> Bonuses { get; set; } = new List<Bonus>();
}
