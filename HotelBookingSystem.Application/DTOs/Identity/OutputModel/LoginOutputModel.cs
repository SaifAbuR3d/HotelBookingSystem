﻿namespace HotelBookingSystem.Application.DTOs.Identity.OutputModel;

public class LoginOutputModel
{
    public LoginOutputModel(string userId, string token)
    {
        UserId = userId;
        Token = token;
    }

    public string UserId { get; }

    public string Token { get; }
}
