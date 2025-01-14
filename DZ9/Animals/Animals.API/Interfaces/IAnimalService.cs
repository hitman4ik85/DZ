using Animals.API.Models;

namespace Animals.API.Interfaces;

public interface IAnimalService
{
    void AddAnimal(Animal animals);
    IEnumerable<Animal> GetAnimals();
    Animal GetAnimalById(int id);
}
