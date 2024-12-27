using Microsoft.AspNetCore.Mvc;
using DZ6.API.DTOs;
using DZ6.Core.Interfaces;
using DZ6.Core.Models;

namespace DZ6.API.Controllers;

[ApiController]
[Route("api/v1/clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ClientDTO>> GetClients([FromQuery] string? filter, [FromQuery] int skip, [FromQuery] int take)
    {
        var clients = _clientService.GetClients(filter, skip, take)
            .Select(c => new ClientDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DateOfBirth = c.DateOfBirth
            });

        return Ok(clients);
    }

    [HttpGet("{id}")]
    public ActionResult<ClientDTO> GetClientById(int id)
    {
        var client = _clientService.GetClientById(id);
        if (client == null)
            return NotFound($"Client with ID {id} not found.");

        var clientDTO = new ClientDTO()
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            DateOfBirth = client.DateOfBirth
        };

        return Ok(clientDTO);
    }

    [HttpPost]
    public ActionResult<ClientDTO> AddClient([FromBody] CreateClientDTO createClientDTO)
    {
        var client = new Client()
        {
            FirstName = createClientDTO.FirstName,
            LastName = createClientDTO.LastName,
            DateOfBirth = createClientDTO.DateOfBirth
        };

        var addedClient = _clientService.AddClient(client);

        var clientDTO = new ClientDTO()
        {
            Id = addedClient.Id,
            FirstName = addedClient.FirstName,
            LastName = addedClient.LastName,
            DateOfBirth = addedClient.DateOfBirth
        };

        return Created($"api/v1/clients/{addedClient.Id}", clientDTO);
    }

    [HttpPut("{id}")]
    public ActionResult<ClientDTO> UpdateClient(int id, [FromBody] CreateClientDTO updateClientDTO)
    {
        var client = new Client()
        {
            Id = id,
            FirstName = updateClientDTO.FirstName,
            LastName = updateClientDTO.LastName,
            DateOfBirth = updateClientDTO.DateOfBirth
        };

        var updatedClient = _clientService.UpdateClient(client);

        var clientDTO = new ClientDTO()
        {
            Id = updatedClient.Id,
            FirstName = updatedClient.FirstName,
            LastName = updatedClient.LastName,
            DateOfBirth = updatedClient.DateOfBirth
        };

        return Ok(clientDTO);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteClient(int id)
    {
        try
        {
            _clientService.DeleteClient(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
