using WorkerManagement.Core.DTOs;

namespace WorkerManagement.Core.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto employeeDto, CancellationToken cancellationToken = default);
    Task<EmployeeDto?> UpdateEmployeeAsync(int id, CreateEmployeeDto employeeDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkHoursDto>> GetEmployeeWorkHoursAsync(int employeeId, int month, int year, CancellationToken cancellationToken = default);
    Task<decimal> CalculateEmployeeSalaryAsync(int employeeId, int month, int year, CancellationToken cancellationToken = default);
}
