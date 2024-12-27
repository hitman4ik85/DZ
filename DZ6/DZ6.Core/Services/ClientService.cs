using DZ6.Core.Interfaces;
using DZ6.Core.Models;

namespace DZ6.Core.Services;

public class ClientService : IClientService
{
    private readonly IRepository _repository;

    public ClientService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Client> GetClients(string? filter, int skip, int take)
    {
        var clientsQuery = _repository.GetAll<Client>().AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            clientsQuery = clientsQuery.Where(c => c.FirstName.Contains(filter) || c.LastName.Contains(filter));
        }

        return clientsQuery
            .OrderBy(c => c.LastName)
            .Skip(skip)
            .Take(take)
            .ToArray();
    }

    public Client GetClientById(int id)
    {
        return _repository.GetAll<Client>().FirstOrDefault(c => c.Id == id);
    }

    public Client AddClient(Client client)
    {
        var addedClient = _repository.Add(client);
        _repository.SaveChanges();
        return addedClient;
    }

    public Client UpdateClient(Client client)
    {
        var updatedClient = _repository.Update(client);
        _repository.SaveChanges();
        return updatedClient;
    }

    public void DeleteClient(int id)
    {
        var client = GetClientById(id);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID {id} not found.");

        _repository.Delete(client);
        _repository.SaveChanges();
    }
}
