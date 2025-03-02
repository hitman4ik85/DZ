using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.API.Controllers;

[Route("api/rooms")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetRooms(CancellationToken cancellationToken = default)
    {
        return Ok(await _roomService.GetRoomsAsync(cancellationToken));
    }

    [HttpGet("{roomId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoomById(int roomId, CancellationToken cancellationToken = default)
    {
        return Ok(await _roomService.GetRoomByIdAsync(roomId, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomDTO dto, CancellationToken cancellationToken = default)
    {
        return Ok(await _roomService.AddRoomAsync(dto, cancellationToken));
    }

    [HttpPut("{roomId}")]
    public async Task<IActionResult> UpdateRoom(int roomId, [FromBody] AddRoomDTO dto, CancellationToken cancellationToken = default)
    {
        return Ok(await _roomService.UpdateRoomAsync(roomId, dto, cancellationToken));
    }

    [HttpDelete("{roomId}")]
    public async Task<IActionResult> DeleteRoom(int roomId, CancellationToken cancellationToken = default)
    {
        await _roomService.DeleteRoomAsync(roomId, cancellationToken);
        return NoContent();
    }
}