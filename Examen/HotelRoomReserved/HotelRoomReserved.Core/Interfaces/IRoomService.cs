using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.Core.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDTO>> GetRoomsAsync(CancellationToken cancellationToken = default);
    Task<RoomDTO> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RoomDTO> AddRoomAsync(AddRoomDTO dto, CancellationToken cancellationToken = default);
    Task<RoomDTO> UpdateRoomAsync(int id, AddRoomDTO dto, CancellationToken cancellationToken = default);
    Task DeleteRoomAsync(int id, CancellationToken cancellationToken = default);
}
