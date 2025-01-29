namespace HotelRoomReserved.Entities.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public ReservationStatus Status { get; set; }
}
