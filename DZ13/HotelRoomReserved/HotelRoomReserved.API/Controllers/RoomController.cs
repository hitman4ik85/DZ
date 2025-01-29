using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Core.Interfaces;

namespace HotelRoomReserved.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms(CancellationToken cancellationToken = default)
    {
        var rooms = await _roomService.GetRoomsAsync();
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDTO>> AddRoom([FromBody] AddRoomDTO roomDto, CancellationToken cancellationToken = default)
    {
        var createdRoom = await _roomService.AddRoomAsync(roomDto);
        return CreatedAtAction(nameof(GetRooms), new { id = createdRoom.Id }, createdRoom);
    }
}
