using System.Security.Cryptography;
using System.Text;

namespace HotelRoomReserved.Core;

public static class PasswordHasher
{
    public static string GenerateHash(string password, DateTime dateTime)
    {
        var salt = dateTime.ToLongTimeString();
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashedBytes);
    }
}