using Animals.API.Interfaces;
using Animals.API.Models;

namespace Animals.API.Services;

public class AnimalService : IAnimalService
{
    private readonly List<Animal> _animals;
    private static int _currentId = 1;
    public AnimalService()
    {
        _animals = new List<Animal>();
    }
    public void AddAnimal(Animal animals)
    {
        animals.Id = _currentId++;
        _animals.Add(animals);
    }

    public IEnumerable<Animal> GetAnimals()
    {
        return _animals;
    }

    public Animal GetAnimalById(int id)
    {
        return _animals.Single(a => a.Id == id);
    }
}
