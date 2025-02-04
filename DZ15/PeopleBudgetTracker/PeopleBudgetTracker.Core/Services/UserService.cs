using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Helpers;
using PeopleBudgetTracker.Core.Interfaces;
using PeopleBudgetTracker.Entities.Models;
using PeopleBudgetTracker.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace PeopleBudgetTracker.Core.Services;

public class UserService : IUserService
{
    private readonly PeopleBudgetTrackerContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserService(PeopleBudgetTrackerContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> RegisterUserAsync(CreateUserDTO createUserDto, CancellationToken cancellationToken = default)
    {
        // Валідація email
        if (!Regex.IsMatch(createUserDto.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            throw new ArgumentException("Email is invalid", nameof(createUserDto.Email));
        }

        // Валідація пароля (мінімум 8 символів, 1 буква, 1 цифра, 1 спецсимвол)
        if (!Regex.IsMatch(createUserDto.Password, @"^.*(?=.{8,})(?=.*[a-zA-Z])(?=.*\d)(?=.*[!#$%&? ""]).*$"))
        {
            throw new ArgumentException("Password is invalid", nameof(createUserDto.Password));
        }

        // Перевірка, чи email вже зареєстрований
        if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email.ToLower(), cancellationToken))
        {
            throw new ArgumentException("Email is already taken", nameof(createUserDto.Email));
        }

        var user = new User
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email.ToLower(),
            PasswordHash = PasswordHasher.GenerateHash(createUserDto.Password, DateTime.UtcNow),
            Account = new Account { Balance = 0 },
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return GenerateToken(user);
    }

    public async Task<string> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // Знаходимо користувача за email
        var user = await _context.Users
            .Include(u => u.Account)
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("User not found for email: ", nameof(email));
        }

        // Перевіряємо хеш пароля
        if (!PasswordHasher.VerifyHash(password, user.PasswordHash))
        {
            throw new ArgumentException("Invalid password", nameof(password));
        }

        return GenerateToken(user);
    }

    public async Task<UserDTO> UpdateUserAsync(UserDTO userDto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.Include(u => u.Account)
                                       .FirstOrDefaultAsync(u => u.Id == userDto.Id, cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userDto.Id));
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDTO>(user);
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email)
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
}
