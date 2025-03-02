﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HotelRoomReserved.Entities.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}
