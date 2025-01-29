using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterUserDTO registerUserDTO, CancellationToken cancellationToken = default);
    Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
