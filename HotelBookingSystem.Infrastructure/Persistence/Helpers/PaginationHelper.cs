using HotelBookingSystem.Application.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Helpers;

public static class PaginationHelper
{
    public static void ApplyPagination<T> (ref IQueryable<T> query, int pageNumber, int pageSize)
    {
        query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
    }

    public static async Task<PaginationMetadata> GetPaginationMetadataAsync<T> (IQueryable<T> query, int pageNumber, int pageSize)
    {
        var count = await query.CountAsync();
        return new PaginationMetadata(pageNumber, pageSize, count);
    }
}
