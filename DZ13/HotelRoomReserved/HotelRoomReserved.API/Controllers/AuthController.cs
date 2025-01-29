using Microsoft.AspNetCore.Mvc;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Core.Interfaces;

namespace HotelRoomReserved.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserDTO registerUserDTO, CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.RegisterAsync(registerUserDTO, cancellationToken));
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromQuery] string email, [FromQuery] string password, CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.LoginAsync(email, password, cancellationToken));
    }
}
