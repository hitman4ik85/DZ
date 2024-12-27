using DZ6.Core.Models;

namespace DZ6.Core.Interfaces;

public interface IClientService
{
    IEnumerable<Client> GetClients(string? filter, int skip, int take);
    Client GetClientById(int id);
    Client AddClient(Client client);
    Client UpdateClient(Client client);
    void DeleteClient(int id);
}
