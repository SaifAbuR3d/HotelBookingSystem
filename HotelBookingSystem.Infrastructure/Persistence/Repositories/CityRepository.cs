﻿using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class CityRepository(ApplicationDbContext context) : ICityRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> CityExistsAsync(Guid id)
    {
        return await _context.Cities.AnyAsync(c => c.Id == id);
    }

    public async Task<City> AddCityAsync(City city)
    {
        await _context.Cities.AddAsync(city);
        return city;
    }

    public async Task<CityImage> AddCityImageAsync(City city, CityImage cityImage)
    {
        await _context.AddAsync(cityImage);
        city.Images.Add(cityImage);
        return cityImage;
    }

    public async Task<bool> DeleteCityAsync(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city is null)
        {
            return false;
        }

        _context.Cities.Remove(city);

        return true;
    }

    public async Task<City?> GetCityAsync(Guid id)
    {
        return await _context.Cities.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<City?> GetCityByNameAsync(string name)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IEnumerable<City>> MostVisitedCitiesAsync(int count = 5)
    {
        var cityIds = await _context.Bookings
            .GroupBy(b => b.Hotel.CityId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToListAsync();

        var cities = await _context.Cities
            .Where(c => cityIds.Contains(c.Id))
            .Include(c => c.Images)
            .AsNoTracking()
            .ToListAsync();

        return cities;

    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<(IEnumerable<City>, PaginationMetadata)> GetAllCitiesAsync(GetCitiesQueryParameters request)
    {
        var query = _context.Cities
                    .Include(c => c.Hotels)
                    .AsQueryable();

        SearchInCityNameOrCountryName(ref query, request.SearchTerm);

        SortingHelper.ApplySorting(ref query, request.SortOrder, SortingHelper.GetCitiesSortingCriterion(request));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query, request.PageNumber, request.PageSize);

        PaginationHelper.ApplyPagination(ref query, request.PageNumber, request.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync();

        return (result, paginationMetadata);
    }

    private static void SearchInCityNameOrCountryName(ref IQueryable<City> query, string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.StartsWith(searchTerm) || c.Country.StartsWith(searchTerm));
        }
    }
}
