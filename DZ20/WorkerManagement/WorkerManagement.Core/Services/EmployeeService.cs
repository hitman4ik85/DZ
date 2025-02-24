using WorkerManagement.Core.DTOs;
using WorkerManagement.Core.Interfaces;
using WorkerManagement.Entities.Models;
using WorkerManagement.Storage;
using Microsoft.EntityFrameworkCore;

namespace WorkerManagement.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository _repository;

    public EmployeeService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAll<Employee>()
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                HourlyRate = e.HourlyRate
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _repository.FindByIdAsync<Employee>(id, cancellationToken);
        if (employee == null) return null;

        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            HourlyRate = employee.HourlyRate
        };
    }

    public async Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto employeeDto, CancellationToken cancellationToken = default)
    {
        var employee = new Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            HourlyRate = employeeDto.HourlyRate
        };

        await _repository.AddAsync(employee, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            HourlyRate = employee.HourlyRate
        };
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, CreateEmployeeDto employeeDto, CancellationToken cancellationToken = default)
    {
        var employee = await _repository.FindByIdAsync<Employee>(id, cancellationToken);
        if (employee == null) return null;

        employee.FirstName = employeeDto.FirstName;
        employee.LastName = employeeDto.LastName;
        employee.HourlyRate = employeeDto.HourlyRate;

        await _repository.UpdateAsync(employee, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            HourlyRate = employee.HourlyRate
        };
    }

    public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _repository.FindByIdAsync<Employee>(id, cancellationToken);
        if (employee == null) return false;

        await _repository.DeleteAsync(employee, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<WorkHoursDto>> GetEmployeeWorkHoursAsync(int employeeId, int month, int year, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAll<WorkLog>()
            .Where(w => w.EmployeeId == employeeId && w.WorkDate.Month == month && w.WorkDate.Year == year)
            .OrderBy(w => w.WorkDate)
            .Select(w => new WorkHoursDto
            {
                WorkDate = w.WorkDate,
                HoursWorked = w.HoursWorked
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<decimal> CalculateEmployeeSalaryAsync(int employeeId, int month, int year, CancellationToken cancellationToken = default)
    {
        var workLogs = await _repository.GetAll<WorkLog>()
            .Where(w => w.EmployeeId == employeeId && w.WorkDate.Month == month && w.WorkDate.Year == year)
            .ToArrayAsync(cancellationToken);

        var bonuses = await _repository.GetAll<Bonus>()
            .Where(b => b.EmployeeId == employeeId && b.BonusDate.Month == month && b.BonusDate.Year == year)
            .SumAsync(b => b.Amount, cancellationToken);

        var employee = await _repository.FindByIdAsync<Employee>(employeeId, cancellationToken);
        if (employee == null) throw new InvalidOperationException("Employee not found");

        decimal totalSalary = 0;

        foreach (var workLog in workLogs)
        {
            decimal dailySalary = 0;
            bool isWeekend = workLog.WorkDate.DayOfWeek == DayOfWeek.Saturday || workLog.WorkDate.DayOfWeek == DayOfWeek.Sunday;

            if (isWeekend)
            {
                dailySalary = workLog.HoursWorked * employee.HourlyRate * 1.3m;
            }
            else
            {
                if (workLog.HoursWorked <= 8)
                {
                    dailySalary = workLog.HoursWorked * employee.HourlyRate;
                }
                else
                {
                    dailySalary = (8 * employee.HourlyRate) + ((workLog.HoursWorked - 8) * employee.HourlyRate * 1.5m);
                }
            }

            totalSalary += dailySalary;
        }

        totalSalary += bonuses;

        return totalSalary;
    }
}