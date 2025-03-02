namespace HotelRoomReserved.Core.DTOs;

public class CreateReservationDTO
{
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
