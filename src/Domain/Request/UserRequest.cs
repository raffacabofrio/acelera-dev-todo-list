﻿namespace Domain.Request;
public class UserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class UserUpdate : UserRequest
{
    public int Id { get; set; }
}
