using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerManagement.Entities.Models;

public class Bonus
{
    public int Id { get; set; }
    public DateTime BonusDate { get; set; }
    public decimal Amount { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
