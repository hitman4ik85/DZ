using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.Core.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<ReservationDTO>> GetUserReservationsAsync(int userId, CancellationToken cancellationToken = default);
    Task<ReservationDTO> CreateReservationAsync(CreateReservationDTO reservationDto, CancellationToken cancellationToken = default);
    Task CancelReservationAsync(int reservationId, int userId, CancellationToken cancellationToken = default);
}
