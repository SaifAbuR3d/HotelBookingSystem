using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.DTOs.Identity.OutputModel;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// Controller for Identity related actions
/// </summary>
/// <param name="identityService"></param>

[ApiController]
public class IdentityController(IIdentityService identityService) : ControllerBase
{
    /// <summary>
    /// Register a guest
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("api/register")]
    public async Task<ActionResult> RegisterGuest([FromBody] RegisterUserModel request)
    {
        await identityService.RegisterUser(request, UserRoles.Guest);
        return Ok();
    }


    /// <summary>
    /// Register an admin
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("api/register-admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterUserModel request)
    {
        await identityService.RegisterUser(request, UserRoles.Admin);
        return Ok();
    }


    /// <summary>
    /// login a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("api/login")]
    public async Task<ActionResult<LoginOutputModel>> Login([FromBody] LoginUserModel request)
    {
        var result = await identityService.Login(request);
        return Ok(result);
    }
}
