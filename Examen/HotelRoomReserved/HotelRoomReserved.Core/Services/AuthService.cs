using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Entities.Models;
using HotelRoomReserved.Storage.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using HotelRoomReserved.Core.Security;

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

    public async Task<string> RegisterAdminAsync(RegisterUserDTO dto, CancellationToken cancellationToken = default)
    {
        return await RegisterUserAsync(dto, UserRole.Admin, cancellationToken);
    }

    public async Task<string> RegisterUserAsync(RegisterUserDTO dto, CancellationToken cancellationToken = default)
    {
        return await RegisterUserAsync(dto, UserRole.Customer, cancellationToken);
    }

    private async Task<string> RegisterUserAsync(RegisterUserDTO dto, UserRole role, CancellationToken cancellationToken)
    {
        ValidateUser(dto);

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email.ToLower(), cancellationToken))
        {
            throw new ArgumentException("Email is already taken", nameof(dto.Email));
        }

        var user = _mapper.Map<User>(dto);
        user.Email = user.Email.ToLower();
        user.CreatedAt = DateTime.UtcNow;
        user.Role = role;
        user.Password = PasswordHasher.GenerateHash(dto.Password, user.CreatedAt);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return GenerateToken(user);
    }

    public async Task<string> LoginAsync(LoginDTO dto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower(), cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("User not found for email", nameof(dto.Email));
        }

        var hashedPassword = PasswordHasher.GenerateHash(dto.Password, user.CreatedAt);
        if (user.Password != hashedPassword)
        {
            throw new ArgumentException("Invalid password for user", nameof(dto.Password));
        }

        return GenerateToken(user);
    }

    public async Task UpdateUserRoleAsync(int userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FindAsync([userId], cancellationToken);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found", nameof(userId));
        }

        if (!Enum.TryParse<UserRole>(role, true, out var newRole))
        {
            throw new ArgumentException("Invalid role", nameof(role));
        }

        user.Role = newRole;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(int userId, UpdateUserDTO dto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FindAsync([userId], cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FindAsync([userId], cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        ValidatePagination(page, pageSize);

        var users = await _context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return _mapper.Map<IEnumerable<UserDTO>>(users);
    }

    private void ValidatePagination(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            throw new ArgumentException("Page and pageSize must be greater than zero.");
        }
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

    private void ValidateUser(RegisterUserDTO dto)
    {
        if (!Regex.IsMatch(dto.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            throw new ArgumentException("Email is invalid", nameof(dto.Email));
        }

        if (!Regex.IsMatch(dto.Password, @"^.*(?=.{8,})(?=.*[a-zA-Z])(?=.*\d)(?=.*[!#$%&? ""]).*$"))
        {
            throw new ArgumentException("Password is invalid", nameof(dto.Password));
        }
    }
}