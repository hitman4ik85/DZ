using ProjectUser.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProjectUser.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<User>> GetUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                NickName = "User1",
                Password = "password1",
                DateOfBirth = new DateTime(2020, 1, 1),
                Description = "Test user 1"
            },
            new User
            {
                Id = 2,
                NickName = "User2",
                Password = "password2",
                DateOfBirth = new DateTime(2021, 2, 2),
                Description = "Test user 2"
            }
        };

        return Ok(users); // Status 200
    }

    [HttpGet("{nickname}")]
    public ActionResult<User> GetUserByNickName([FromRoute] string nickname)
    {
        var user = new User
        {
            Id = 1,
            NickName = nickname,
            Password = "password",
            DateOfBirth = new DateTime(2020, 1, 1),
            Description = "Test user"
        };

        return Ok(user); // Status 200
    }

    [HttpPost]
    public ActionResult<User> AddUser([FromBody] User user)
    {
        user.Id = new Random().Next(1, 1000000); // Генерація ID

        return Created($"/users/{user.NickName}", user); // Status 201
    }


    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsersPaged([FromQuery] int skip, [FromQuery] int take)
    {
        var users = new List<User>(capacity: 1000);
        for (int i = 0; i < 1000; i++)
        {
            var user = new User()
            {
                Id = i,
                NickName = $"User{i}",
                Password = "password" + i,
                DateOfBirth = DateTime.Now.AddYears(-20).AddDays(i),
                Description = $"Description for User{i}"
            };
            users.Add(user);
        }

        var pagedUsers = users
            .Skip(skip * take)
            .Take(take);
            
        return Ok(pagedUsers);
    }
}
