using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.DTOs;

namespace HotelRoomReserved.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDTO dto, CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.RegisterAdminAsync(dto, cancellationToken));
    }

    [HttpPost("register-user")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto, CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.RegisterUserAsync(dto, cancellationToken));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto, CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.LoginAsync(dto, cancellationToken));
    }

    [HttpPut("update-role/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] string role, CancellationToken cancellationToken = default)
    {
        await _authService.UpdateUserRoleAsync(userId, role, cancellationToken);
        return NoContent();
    }

    [HttpPut("update-user/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDTO dto, CancellationToken cancellationToken = default)
    {
        await _authService.UpdateUserAsync(userId, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("delete-user/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken = default)
    {
        await _authService.DeleteUserAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.GetUsersAsync(page, pageSize, cancellationToken));
    }
}