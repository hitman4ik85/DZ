using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.Core.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDTO>> GetRoomsAsync(CancellationToken cancellationToken = default);
    Task<RoomDTO> AddRoomAsync(AddRoomDTO roomDto, CancellationToken cancellationToken = default);
}
