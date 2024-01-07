using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.IdentityInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.DTOs.Identity.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Application.Identity;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services;

public class IdentityService(IIdentityManager identityManager, 
                             
                             IGuestRepository guestRepository) : IIdentityService
{
    private readonly IIdentityManager _identityManager = identityManager;
    private readonly IGuestRepository _guestRepository = guestRepository;

    public async Task<LoginOutputModel> Login(LoginUserModel model)
    {
        var user = await _identityManager.Login(model);

        if (user.Role == UserRoles.Admin)
        {
            return new LoginOutputModel(user.UserId.ToString(), user.Token);
        }
        
        var guest = await _guestRepository.GetGuestByUserIdAsync(user.UserId) 
            ?? throw new NotFoundException(nameof(Guest), user.UserId);

        var outputModel = new LoginOutputModel(guest.Id.ToString(), user.Token);
        return outputModel;
    }


    public async Task<IUser> RegisterUser(RegisterUserModel model, string role)
    {
        if (role != UserRoles.Guest && role != UserRoles.Admin)
        {
            throw new BadRequestException($"Invalid role: {role} at RegisterUser");
        }

        var user = await _identityManager.RegisterUser(model, role);

        if(role == UserRoles.Admin)
        {
            return user; 
        }

        var guest = new Guest
        {
            CreationDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var guestEntity = await _guestRepository.AddGuestAsync(guest);

        user.BecomeGuest(guestEntity);

        await _guestRepository.SaveChangesAsync();

        return user;
    }
}
