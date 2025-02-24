using System.Linq.Expressions;

namespace WorkerManagement.Storage;

public interface IRepository
{
    IQueryable<T> GetAll<T>() where T : class;
    Task<T?> FindByIdAsync<T>(int id, CancellationToken cancellationToken = default) where T : class;
    Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
