using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.DTOs.Room.Query;
using HotelBookingSystem.Domain.Models;
using System.Linq.Expressions;

namespace HotelBookingSystem.Infrastructure.Persistence.Helpers;

public static class SortingHelper
{
    public static void ApplySorting<T>(ref IQueryable<T> query,
                                string? sortOrder,
                                Expression<Func<T, object>> keySelector)
    {

        if (sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(keySelector);
        }
        else
        {
            query = query.OrderBy(keySelector);
        }
    }

    public static Expression<Func<Hotel, object>> GetSearchResultsSortingCriterion(HotelSearchAndFilterParameters request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "name" => h => h.Name,
            "price" => h => h.Rooms.Min(r => r.Price),
            "rating" => h => h.Reviews.Average(r => r.Rating),
            "starrating" => h => h.StarRate,
            _ => h => h.Id
        };
    }

    public static Expression<Func<City, object>> GetCitiesSortingCriterion(GetCitiesQueryParameters request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "creationdate" => c => c.CreationDate,
            "lastmodified" => c => c.LastModified,
            "name" => c => c.Name,
            "country" => c => c.Country,
            "postoffice" => c => c.PostOffice,
            "hotels" => c => c.Hotels.Count,
            _ => c => c.Id
        };
    }

    public static Expression<Func<Room, object>> GetRoomsSortingCriterion(GetRoomsQueryParameters request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "creationdate" => r => r.CreationDate,
            "lastmodified" => r => r.LastModified,
            "roomnumber" => r => r.RoomNumber,
            "adultscapacity" => r => r.AdultsCapacity,
            "childrencapacity" => r => r.ChildrenCapacity,
            "hotelname" => r => r.Hotel.Name,
            _ => r => r.Id
        };
    }


}
