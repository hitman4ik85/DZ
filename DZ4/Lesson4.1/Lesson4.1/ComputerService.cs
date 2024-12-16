using Lesson4._1.Data;
using Lesson4._1.Models;

namespace Lesson4._1;

public class ComputerService : IComputerService
{ 							//--- ДепенденсІнжекшен ---
    private readonly IDatabase _computerDatabase;  //передаємо інтерфейс

    public ComputerService(IDatabase computerDatabase) //генеруємо конструктор
    {
        _computerDatabase = computerDatabase; //робимо присвоєння
    }

    public void AddComputer(Computer computer)
    {
        ValidateComputer(computer);

        _computerDatabase.Add(computer);
    }

    public List<Computer> GetComputers()
    {
        return _computerDatabase.GetAll(); //тепер просто повертаємо наші компютера завдяки ДепІнж
    }

    public void RemoveComputer(Computer computer)
    {
        if (!ComputerExists(computer.Id))
        {
            throw new ArgumentException();
        }
        _computerDatabase.Delete(computer);
    }

    public void UpdateComputer(int id, Computer computer)
    {
        ValidateComputer(computer);
        if (!ComputerExists(computer.Id))
        {
            throw new ArgumentException();
        }
        _computerDatabase.Update(id, computer);
    }

    public void ValidateComputer(Computer computer)
    {
        if (string.IsNullOrEmpty(computer.Name))
        {
            throw new ArgumentException();
        }
    }

    private bool ComputerExists(int id)
    {
        return _computerDatabase.GetAll().Any(x => x.Id == id);

    }
}