namespace HotelRoomReserved.Core.DTOs;

public class ReservationDTO
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string Status { get; set; }
}
