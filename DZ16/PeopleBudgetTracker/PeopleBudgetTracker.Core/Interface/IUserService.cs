using PeopleBudgetTracker.Core.DTOs;

namespace PeopleBudgetTracker.Core.Interfaces;

public interface IUserService
{
    Task<string> RegisterUserAsync(CreateUserDTO createUserDto, CancellationToken cancellationToken = default);
    Task<string> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<UserDTO> UpdateUserAsync(UserDTO userDto, CancellationToken cancellationToken = default);
}
