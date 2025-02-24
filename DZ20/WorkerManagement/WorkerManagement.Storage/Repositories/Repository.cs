using Microsoft.EntityFrameworkCore;
using WorkerManagement.Storage.Data;
using System.Linq.Expressions;

namespace WorkerManagement.Storage;

public class Repository : IRepository
{
    private readonly WorkerManagementContext _context;

    public Repository(WorkerManagementContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll<T>() where T : class
    {
        return _context.Set<T>();
    }

    public async Task<T?> FindByIdAsync<T>(int id, CancellationToken cancellationToken = default) where T : class
    {
        return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
