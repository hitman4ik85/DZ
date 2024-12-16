using Lesson4._1.Models;

namespace Lesson4._1.Data;

public interface IDatabase
{
    List<Computer> GetAll();
    void Add(Computer computer);
    void Delete(Computer computer);
    void Update(int id, Computer computer);
}
