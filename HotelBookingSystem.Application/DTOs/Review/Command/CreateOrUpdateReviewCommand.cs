﻿namespace HotelBookingSystem.Application.DTOs.Review.Command;

public class CreateOrUpdateReviewCommand
{
    public string? Title { get; set; }
    public string Description { get; set; } = default!;
    public int Rating { get; set; }
}