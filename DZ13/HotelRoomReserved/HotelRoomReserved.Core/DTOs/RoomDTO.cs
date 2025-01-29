﻿namespace HotelRoomReserved.Core.DTOs;

public class RoomDTO
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}
