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

    private readonly List<string> validRoles = [UserRoles.Admin, UserRoles.Guest];

    public async Task<LoginSuccessModel> Login(LoginUserModel model)
    {
        var user = await CheckCredentials(model);

        var roles = await GetUserRoles(user);

        var token = jwtTokenGenerator.GenerateToken(user, roles)
            ?? throw new TokenGenerationFailedException("null token");

        return new LoginSuccessModel(user.Id, token, roles.First());
    }

    private async Task<ApplicationUser> CheckCredentials(LoginUserModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new InvalidUserCredentialsException();
        }

        return user;
    }

    private async Task<IList<string>> GetUserRoles(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            throw new NoRolesException(user.Id);
        }

        return roles;
    }



    public async Task<IUser> RegisterUser(RegisterUserModel model, string role)
    {
        ValidateRole(role);

        var user = new ApplicationUser(model.Username, model.Email);

        var identityResult = await _userManager.CreateAsync(user, model.Password);
        if (!identityResult.Succeeded)
        {
            var errors = JsonSerializer.Serialize(identityResult.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        await EnsureRolesExist();
        await _userManager.AddToRoleAsync(user, role);

        return user;
    }

    private void ValidateRole(string role)
    {
        if (!validRoles.Contains(role))
        {
            throw new BadRequestException($"Invalid role: {role}");
        }
    }

    private async Task EnsureRolesExist()
    {
        foreach(var role in validRoles)
        {
            await CreateRoleIfNotExists(role);
        }
    }

    private async Task CreateRoleIfNotExists(string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
