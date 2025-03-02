using System.ComponentModel.DataAnnotations.Schema;

namespace HotelRoomReserved.Entities.Models;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}
