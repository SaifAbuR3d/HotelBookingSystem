using Azure.Core;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public static Expression<Func<City, object>> GetCitySortingCriterion(ResourceQueryParameters request)
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


}
