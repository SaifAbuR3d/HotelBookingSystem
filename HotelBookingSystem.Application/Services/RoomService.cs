using AutoMapper;
using HotelBookingSystem.Application.DTOs.Room;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Application.ServiceInterfaces;
using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services;

public class RoomService : IRoomService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    public RoomService(IHotelRepository hotelRepository,
                       IRoomRepository roomRepository,
                       IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomOutputModel>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllRoomsAsync();
        var mapped = _mapper.Map<IEnumerable<RoomOutputModel>>(rooms);

        return mapped;
    }

    public async Task<RoomOutputModel?> GetRoomAsync(Guid id)
    {
        var room = await _roomRepository.GetRoomAsync(id) ?? throw new NotFoundException(nameof(Room), id);
        var mapped = _mapper.Map<RoomOutputModel>(room);
        return mapped;
    }

    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        var deleted = await _roomRepository.DeleteRoomAsync(id);
        if (deleted)
        {
            await _roomRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<RoomOutputModel> CreateRoomAsync(CreateRoomCommand request)
    {
        var hotel = await _hotelRepository.GetHotelByNameAsync(request.HotelName);
        if (hotel is null)
        {
            throw new NotFoundException(nameof(hotel), request.HotelName); 
        }

        var room = _mapper.Map<Room>(request);

        room.Id = Guid.NewGuid();
        room.CreationDate = DateTime.UtcNow;
        room.LastModified = DateTime.UtcNow;
        room.Hotel = hotel;

        var createdRoom = await _roomRepository.AddRoomAsync(room);
        await _roomRepository.SaveChangesAsync();

        return _mapper.Map<RoomOutputModel>(createdRoom);
    }

    public async Task<bool> UpdateRoomAsync(Guid id, UpdateRoomCommand request)
    {
        var room = await _roomRepository.GetRoomAsync(id) ?? throw new NotFoundException(nameof(Room), id);
        _mapper.Map(request, room);

        var updated = await _roomRepository.SaveChangesAsync();
        return updated;
    }
}
