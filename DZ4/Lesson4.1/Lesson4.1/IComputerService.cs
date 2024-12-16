using Lesson4._1.Models;

namespace Lesson4._1;

public interface IComputerService
{
    List<Computer> GetComputers();
    void AddComputer(Computer computer);
    void RemoveComputer(Computer computer);
    void UpdateComputer(int id, Computer computer);
}