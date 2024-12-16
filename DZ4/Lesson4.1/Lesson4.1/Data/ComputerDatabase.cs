using Lesson4._1.Models;

namespace Lesson4._1.Data;

public class ComputerDatabase : IDatabase
{
    private readonly List<Computer> _computers = new List<Computer>();
    private static int _counter = 0;

    public void Add(Computer computer)
    {
        computer.Id = ++_counter;
        _computers.Add(computer);
    }

    public void Delete(Computer computer)
    {
        _computers.RemoveAll(x => x.Id == computer.Id);
    }

    public List<Computer> GetAll()
    {
        return _computers;
    }

    public void Update(int id, Computer computer)
    {
        int index = _computers.FindIndex(x => x.Id == id);
        _computers[index] = computer;
    }
}
