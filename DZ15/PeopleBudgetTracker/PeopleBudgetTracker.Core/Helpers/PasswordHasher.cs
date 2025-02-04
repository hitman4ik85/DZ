using System.Security.Cryptography;
using System.Text;

namespace PeopleBudgetTracker.Core.Helpers;

public static class PasswordHasher
{
    public static string GenerateHash(string password, DateTime createdAt)
    {
        var salt = createdAt.ToLongTimeString();
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashedBytes);
    }

    public static bool VerifyHash(string password, string hashedPassword)
    {
        return GenerateHash(password, DateTime.UtcNow) == hashedPassword;
    }
}
