using System.Text.RegularExpressions;
using UnitTestProjectDZ17.Models;

namespace UnitTestProjectDZ17.Service;

public class UserService
{
    private readonly List<User> _users = new();

    public bool IsUserValid(User user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName) ||
            string.IsNullOrWhiteSpace(user.LastName) ||
            string.IsNullOrWhiteSpace(user.Email) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            return false;
        }

        if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return false;
        }

        if (!Regex.IsMatch(user.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$"))
        {
            return false;
        }

        return true;
    }

    public void AddUser(User user)
    {
        if (!IsUserValid(user))
        {
            throw new ArgumentException("User is not valid");
        }

        _users.Add(user);
    }

    public void RemoveUserByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        var user = _users.FirstOrDefault(u => u.FirstName == name);
        if (user == null)
        {
            throw new ArgumentNullException("User not found");
        }

        _users.Remove(user);
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }
}
