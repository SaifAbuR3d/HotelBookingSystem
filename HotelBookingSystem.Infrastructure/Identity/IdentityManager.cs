using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.IdentityInterfaces;
using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.DTOs.Identity.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace HotelBookingSystem.Infrastructure.Identity;

public class IdentityManager(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             IJwtTokenGenerator jwtTokenGenerator) : IIdentityManager
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<LoginSuccessModel> Login(LoginUserModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidUserCredentialsException();

        bool result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!result)
        {
            throw new InvalidUserCredentialsException();
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count == 0)
        {
            throw new NoRolesException(user.Id);
        }

        var token = jwtTokenGenerator.GenerateToken(user, roles);

        return new LoginSuccessModel(user.Id, token, roles.First()); 
    }

    public async Task<IUser> RegisterUser(RegisterUserModel model, string role)
    {
        var user = new ApplicationUser(model.Username, model.Email);

        var identityResult = await _userManager.CreateAsync(user, model.Password);

        if (!identityResult.Succeeded)
        {
            var errors = JsonSerializer.Serialize(identityResult.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        await CreateRoles();

        await _userManager.AddToRoleAsync(user, role);

        return user;
    }

    private async Task CreateRoles()
    {
        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        if (!await roleManager.RoleExistsAsync(UserRoles.Guest))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Guest));
    }
}
