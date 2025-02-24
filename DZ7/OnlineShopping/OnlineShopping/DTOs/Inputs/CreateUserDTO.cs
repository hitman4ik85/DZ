﻿namespace OnlineShopping.DTOs.Inputs;

public class CreateUserDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
}
