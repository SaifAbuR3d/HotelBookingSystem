﻿using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IGuestService
{
    Task<IEnumerable<RecentlyVisitedHotelOutputModel>> GetRecentlyVisitedHotelsAsync(Guid guestId, int count = 5);
}