using DZ6.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Storage.Repositories;
//він буде працювати з будь-якими таблицями
public class GenericRepository : IRepository //дуже важливо його наслідувати його від інтерфейсу IRepository який лежить у нашому OnlineShopProject.Core
{
    private readonly DZ6Context _context; //це робимо для того щоб це все робити через ДепенденсІнжекшен і в одному місці мати ці всі налаштування

    public GenericRepository(DZ6Context context) //проініціалізуємо через конструктор
    {
        _context = context;
    }
    //маємо методи для роботи з нашою Базою Даних:
    public IQueryable<T> GetAll<T>() where T : class //отримати усіх, з параметром <Т>, тобто ми наперед не знаємо що нам потрібно повертати
    {
        return _context.Set<T>(); //на основі типу даних Set сам розуміє яку таблицю нам потрібно передати, він має бути обов'язково референс типом
    }                 //воно накладається таким чином (where T : class), його треба зробити і для нашого IRepository.cs
                      //це дає обмеження для нашого тиму <T>, щоб можно передати тільки конкретно референс тип(тобто класи)
    public T Add<T>(T item) where T : class
    {
        var entity = _context.Add(item); //потрібно дістати entity який ми в базі даних додали

        return entity.Entity; //він буде не змінений доки не викличемо SaveChanges()
    }

    public T Update<T>(T item) where T : class
    {
        var entity = _context.Update(item); //викликаємо для нашого елемента(entity) готовий метод Update()

        return entity.Entity; //повернути його
    }

    public void Delete<T>(T item) where T : class //по типу воно саме зрозуміє, що треба видаляти
    {
        _context.Remove(item); //видаляємо об'єкт який є в середині нашої таблиці
    }

    public void SaveChanges()
    {
        _context.SaveChanges(); //збереження змін у Базі Даних(працює як транзакція)
    }
} //тепер це все будемо використовувати в нашому сервісі
