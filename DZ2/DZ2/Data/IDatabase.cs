namespace DZ2.Data;

public interface IDatabase<T>  // Інтерфейс для роботи з даними
{
    IEnumerable<T> Get(); // Отримати всі
    void Add(T item); // Додати
    void Update(T oldItem, T newItem); // Оновити
    void Remove(T item); // Видалити
}
