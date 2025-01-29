using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Entities.Models;
using HotelRoomReserved.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace HotelRoomReserved.Core.Services;

public class AuthService : IAuthService
{
    private readonly HotelContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(HotelContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(RegisterUserDTO registerUserDTO, CancellationToken cancellationToken = default)
    {
        ValidateUser(registerUserDTO);

        if (await _context.Users.AnyAsync(x => x.Email.Equals(registerUserDTO.Email.ToLower()), cancellationToken))
        {
            throw new ArgumentException("Email is already taken", nameof(registerUserDTO.Email));
        }

        var user = _mapper.Map<User>(registerUserDTO);
        user.Email = user.Email.ToLower();
        user.Role = UserRole.Customer;
        user.Password = PasswordHasher.GenerateHash(registerUserDTO.Password, DateTime.UtcNow);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return GenerateToken(user);
    }

    public async Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("User not found for email: ", nameof(email));
        }

        var hashedPassword = PasswordHasher.GenerateHash(password, DateTime.UtcNow);
        if (user.Password != hashedPassword)
        {
            throw new ArgumentException("Invalid password for user: ", nameof(user.Password));
        }

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration.GetSection("AppSettings:ExpireTime").Value!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void ValidateUser(RegisterUserDTO registerUserDTO)
    {
        if (!Regex.IsMatch(registerUserDTO.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            throw new ArgumentException("Email is invalid", nameof(registerUserDTO.Email));
        }

        if (!Regex.IsMatch(registerUserDTO.Password, @"^.*(?=.{8,})(?=.*[a-zA-Z])(?=.*\d)(?=.*[!#$%&? ""]).*$"))
        {
            throw new ArgumentException("Password is invalid", nameof(registerUserDTO.Password));
        }
    }
}
