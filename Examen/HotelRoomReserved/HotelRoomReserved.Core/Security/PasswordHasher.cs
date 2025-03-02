using System.Security.Cryptography;
using System.Text;

namespace HotelRoomReserved.Core.Security;

public static class PasswordHasher
{
    public static string GenerateHash(string password, DateTime createdAt)
    {
        var salt = createdAt.ToLongTimeString();
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashedBytes);
    }
}
