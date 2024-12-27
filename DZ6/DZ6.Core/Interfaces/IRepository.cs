using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Interfaces;

public interface IRepository //інтерфейс для роботи з нашою Базою Даних, потрібен для того щоб адаптувати під нього будь який свій клас
{
    IQueryable<T> GetAll<T>() where T : class; //для зберігання даних у вигляді скрипта, щоб не тримати все в оперативній пам'яті

    T Add<T>(T item) where T : class; //добавити елемент до бази даних (за дженерік методом <T>)
    T Update<T>(T item) where T : class; //оновлення
    void Delete<T>(T item) where T : class; //видалення
    void SaveChanges(); //для зберігання змін
}
