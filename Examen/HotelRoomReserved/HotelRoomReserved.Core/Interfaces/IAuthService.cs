using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAdminAsync(RegisterUserDTO dto, CancellationToken cancellationToken = default);
    Task<string> RegisterUserAsync(RegisterUserDTO dto, CancellationToken cancellationToken = default);
    Task<string> LoginAsync(LoginDTO dto, CancellationToken cancellationToken = default);
    Task UpdateUserRoleAsync(int userId, string role, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(int userId, UpdateUserDTO dto, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDTO>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
