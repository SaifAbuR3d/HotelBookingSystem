using Serilog;
using HotelBookingSystem.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystem.Application.DTOs.Hotel.Query;

namespace HotelBookingSystem.Api.Helpers;

/// <summary>
/// static class for adding next and previous page links to pagination metadata
/// </summary>
public static class PageLinker
{
    /// <summary>
    /// Add next and previous page links to the passed pagination metadata (for general ResourceQueryParameter)
    /// </summary>
    /// <param name="url"></param>
    /// <param name="routeName"></param>
    /// <param name="paginationMetadata"></param>
    /// <param name="parameters"></param>
    public static void AddPageLinks(IUrlHelper url, string routeName,
        PaginationMetadata paginationMetadata, ResourceQueryParameters parameters)
    {
        Log.Debug("AddPageLinks started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);

        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ?
            GetPageLink(url, routeName, paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ?
            GetPageLink(url, routeName, paginationMetadata, parameters, next: true) : null;

        Log.Debug("AddPageLinks for query: {@parameters}, with pagination metadata: {@paginationMetadata} completed successfully", parameters, paginationMetadata);

    }

    private static string? GetPageLink(IUrlHelper url, string routeName,
        PaginationMetadata paginationMetadata, ResourceQueryParameters parameters, bool next)
    {
        if (next)
        {
            Log.Debug("CreatePageLinks for the next page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }
        else
        {
            Log.Debug("CreatePageLinks for the previous page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }

        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        var link =
            url.Link(routeName, new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,
            });

        return link;
    }


    /// <summary>
    /// Add next and previous page links to the passed pagination metadata (for specific HotelSearchAndFilterParameters)
    /// used in SearchHotels
    /// </summary>
    /// <param name="url"></param>
    /// <param name="routeName"></param>
    /// <param name="paginationMetadata"></param>
    /// <param name="parameters"></param>
    public static void AddPageLinks(IUrlHelper url, string routeName,
        PaginationMetadata paginationMetadata, HotelSearchAndFilterParameters parameters)
    {
        Log.Debug("AddPageLinks started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);

        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ?
            GetPageLink(url, routeName,paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ?
            GetPageLink(url, routeName, paginationMetadata, parameters, next: true) : null;

        Log.Debug("AddPageLinks for query: {@parameters}, with pagination metadata: {@paginationMetadata} completed successfully", parameters, paginationMetadata);

    }

    private static string? GetPageLink(IUrlHelper url, string routeName,
        PaginationMetadata paginationMetadata, HotelSearchAndFilterParameters parameters, bool next)
    {
        if (next)
        {
            Log.Debug("CreatePageLinks for the next page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }
        else
        {
            Log.Debug("CreatePageLinks for the previous page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }

        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        var link =
            url.Link(routeName, new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,

                checkInDate = parameters.CheckInDate,
                checkOutDate = parameters.CheckOutDate,
                adults = parameters.Adults,
                children = parameters.Children,
                rooms = parameters.Rooms,

                minStarRating = parameters.MinStarRating,
                maxPrice = parameters.MaxPrice,
                minPrice = parameters.MinPrice,
                amenities = parameters?.Amenities,
                roomTypes = parameters?.RoomTypes
            });

        return link;
    }

}
