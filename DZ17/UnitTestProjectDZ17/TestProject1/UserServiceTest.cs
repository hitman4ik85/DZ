using Xunit;
using UnitTestProjectDZ17.Models;
using UnitTestProjectDZ17.Service;

namespace TestProject1;

public class UserServiceTest
{
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _userService = new UserService();
    }

    [Theory]
    [InlineData("Lane", "Osmon", "lm@gmail.com", "Valid1@#", true)]
    [InlineData("", "Osmon", "lm@gmail.com", "Valid1@#", false)]
    [InlineData("Lane", "", "lm@gmail.com", "Valid1@#", false)]
    [InlineData("Lane", "Osmon", "", "Valid1@#", false)]
    [InlineData("Lane", "Osmon", "lm@gmail.com", "", false)]
    [InlineData("Lane", "Osmon", "invalidgmail.com", "Valid1@#", false)]
    [InlineData("Lane", "Osmon", "lm@gmail.com", "short1@", false)]
    [InlineData("Lane", "Osmon", "lm@gmail.com", "NoNumber!", false)]
    [InlineData("Lane", "Osmon", "lm@gmail.com", "NONUMBER1", false)]
    [InlineData("Lane", "Osmon", "lm@gmail.com", "nonumber1!", false)]
    public void IsUserValid_RandomInput_CorrectValidation(string firstName, string lastName, string email, string password, bool expected)
    {
        var user = new User { FirstName = firstName, LastName = lastName, Email = email, Password = password };

        var actual = _userService.IsUserValid(user);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AddUser_InvalidUser_ThrowsException()
    {
        var user = new User { FirstName = "Lane", LastName = "", Email = "lm@gmail.com", Password = "Valid1@#" };

        var exceptionHandler = () => _userService.AddUser(user);

        Assert.Throws<ArgumentException>(exceptionHandler);
    }

    [Fact]
    public void RemoveUserByName_EmptyName_ThrowsException()
    {
        var exceptionHandler = () => _userService.RemoveUserByName("");

        Assert.Throws<ArgumentException>(exceptionHandler);
    }

    [Fact]
    public void RemoveUserByName_UserNotFound_ThrowsException()
    {
        var exceptionHandler = () => _userService.RemoveUserByName("NonExistent");

        Assert.Throws<ArgumentNullException>(exceptionHandler);
    }

    [Fact]
    public void RemoveUserByName_ValidUser_RemovesSuccessfully()
    {
        var user = new User { FirstName = "Lane", LastName = "Osmon", Email = "lm@gmail.com", Password = "Valid1@#" };
        _userService.AddUser(user);

        _userService.RemoveUserByName("Lane");

        var exceptionHandler = () => _userService.RemoveUserByName("Lane");
        Assert.Throws<ArgumentNullException>(exceptionHandler);
    }

    [Fact]
    public void AddUser_ValidUser_UserIsAdded()
    {
        var user = new User { FirstName = "Lane", LastName = "Osmon", Email = "lm@gmail.com", Password = "Valid1@#" };
        _userService.AddUser(user);

        var users = _userService.GetAllUsers();
        Assert.Contains(users, u => u.FirstName == "Lane" && u.LastName == "Osmon");
    }
}
