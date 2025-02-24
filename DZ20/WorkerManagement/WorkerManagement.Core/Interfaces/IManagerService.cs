using WorkerManagement.Core.DTOs;

namespace WorkerManagement.Core.Interfaces;

public interface IManagerService
{
    Task<IEnumerable<ManagerDto>> GetAllManagersAsync(CancellationToken cancellationToken = default);
    Task<ManagerDto?> GetManagerByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ManagerDto> AddManagerAsync(CreateManagerDto managerDto, CancellationToken cancellationToken = default);
    Task<ManagerDto?> UpdateManagerAsync(int id, CreateManagerDto managerDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteManagerAsync(int id, CancellationToken cancellationToken = default);
}
