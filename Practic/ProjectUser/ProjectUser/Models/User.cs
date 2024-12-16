namespace ProjectUser.Models;

public class User
{
    public int Id { get; set; }
    public string? NickName { get; set; }
    public string? Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Description { get; set; }
}
