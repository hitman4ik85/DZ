using Animals.API.Interfaces;
using Animals.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Controllers;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalsController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpPost]
    public IActionResult AddAnimal([FromBody] Animal animal)
    {
        _animalService.AddAnimal(animal);
        return Created();
    }

    [HttpGet]
    public IActionResult GetAnimals()
    {
        return Ok(_animalService.GetAnimals());
    }

    [HttpGet("{id}")]
    public IActionResult GetAnimalById([FromRoute] int id)
    {
        return Ok(_animalService.GetAnimalById(id));
    }
}