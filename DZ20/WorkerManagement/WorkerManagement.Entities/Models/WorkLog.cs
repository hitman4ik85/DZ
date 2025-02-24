using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerManagement.Entities.Models;

public class WorkLog
{
    public int Id { get; set; }
    public DateTime WorkDate { get; set; }
    public int HoursWorked { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
