using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Room;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Services;

public class RoomService(IHotelRepository hotelRepository,
                         IRoomRepository roomRepository,
                         IMapper mapper,
                         IImageHandler imageHandler) : IRoomService
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageHandler _imageHandler = imageHandler;

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
        var hotel = await _hotelRepository.GetHotelByNameAsync(request.HotelName) ?? throw new NotFoundException(nameof(Hotel), request.HotelName);


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

        await _roomRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UploadImageAsync(Guid roomId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new BadFileException("an error occurred");
        }

        var room = await _roomRepository.GetRoomAsync(roomId) ?? throw new NotFoundException(nameof(Room), roomId);

        var roomDirectory = Path.Combine(basePath, "images", "rooms", roomId.ToString());

        var uploadedImageUrl = await _imageHandler.UploadImage(file, roomDirectory, thumbnail.GetValueOrDefault(false));

        var image = new RoomImage
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            ImageUrl = uploadedImageUrl,
            AlternativeText = alternativeText,
            RoomId = room.Id
        };

        await _roomRepository.AddRoomImageAsync(room, image);
        await _roomRepository.SaveChangesAsync();

        return true;
    }


}
