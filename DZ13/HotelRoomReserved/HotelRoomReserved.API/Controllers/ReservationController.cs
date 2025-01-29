using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Core.Interfaces;

namespace HotelRoomReserved.API.Controllers;

[Authorize]
[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetUserReservations(CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        var reservations = await _reservationService.GetUserReservationsAsync(userId, cancellationToken);
        return Ok(reservations);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAllReservations(CancellationToken cancellationToken = default)
    {
        var reservations = await _reservationService.GetUserReservationsAsync(0, cancellationToken);
        return Ok(reservations);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDTO>> CreateReservation([FromBody] CreateReservationDTO reservationDto, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        reservationDto.UserId = userId;

        var createdReservation = await _reservationService.CreateReservationAsync(reservationDto, cancellationToken);
        return CreatedAtAction(nameof(GetUserReservations), new { id = createdReservation.Id }, createdReservation);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelReservation(int id, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        await _reservationService.CancelReservationAsync(id, userId, cancellationToken);
        return NoContent();
    }
}
