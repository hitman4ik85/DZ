namespace DZ3.Data;

public interface IDatabase<T>
{
    IEnumerable<T> Get();
    void Add(T item);
    void Update(T oldItem, T newItem);
    void Remove(T item);
}
