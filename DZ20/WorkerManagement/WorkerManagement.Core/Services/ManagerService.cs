using WorkerManagement.Core.DTOs;
using WorkerManagement.Core.Interfaces;
using WorkerManagement.Entities.Models;
using WorkerManagement.Storage;
using Microsoft.EntityFrameworkCore;

namespace WorkerManagement.Core.Services;

public class ManagerService : IManagerService
{
    private readonly IRepository _repository;

    public ManagerService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ManagerDto>> GetAllManagersAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAll<Manager>()
            .Select(m => new ManagerDto
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<ManagerDto?> GetManagerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var manager = await _repository.FindByIdAsync<Manager>(id, cancellationToken);
        if (manager == null) return null;

        return new ManagerDto
        {
            Id = manager.Id,
            FirstName = manager.FirstName,
            LastName = manager.LastName
        };
    }

    public async Task<ManagerDto> AddManagerAsync(CreateManagerDto managerDto, CancellationToken cancellationToken = default)
    {
        var manager = new Manager
        {
            FirstName = managerDto.FirstName,
            LastName = managerDto.LastName
        };

        await _repository.AddAsync(manager, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new ManagerDto
        {
            Id = manager.Id,
            FirstName = manager.FirstName,
            LastName = manager.LastName
        };
    }

    public async Task<ManagerDto?> UpdateManagerAsync(int id, CreateManagerDto managerDto, CancellationToken cancellationToken = default)
    {
        var manager = await _repository.FindByIdAsync<Manager>(id, cancellationToken);
        if (manager == null) return null;

        manager.FirstName = managerDto.FirstName;
        manager.LastName = managerDto.LastName;

        await _repository.UpdateAsync(manager, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new ManagerDto
        {
            Id = manager.Id,
            FirstName = manager.FirstName,
            LastName = manager.LastName
        };
    }

    public async Task<bool> DeleteManagerAsync(int id, CancellationToken cancellationToken = default)
    {
        var manager = await _repository.FindByIdAsync<Manager>(id, cancellationToken);
        if (manager == null) return false;

        await _repository.DeleteAsync(manager, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}