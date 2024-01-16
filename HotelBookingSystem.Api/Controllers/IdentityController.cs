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
    /// <remarks> 
    /// Sample request:
    ///
    ///     POST /register
    ///     {
    ///        "username": "Sample_User_Name",
    ///        "firstName": "Sample",
    ///        "lastName": "User",
    ///        "email": "sample@user.com",
    ///        "password": "sample_password"
    ///     }
    ///
    /// </remarks>
    /// <response code="201"></response>
    /// <response code="400">if the request data is invalid</response>
    /// <returns>No Content</returns>
    [HttpPost("api/register")]
    public async Task<ActionResult> RegisterGuest([FromBody] RegisterUserModel request)
    {
        await identityService.RegisterUser(request, UserRoles.Guest);
        return Ok(new { Status = "Success", Message = "User Created Successfully" });
    }


    /// <summary>
    /// Register an admin
    /// </summary>
    /// <param name="request"></param>
    /// <remarks> 
    /// Sample request:
    ///
    ///     POST /register-admin
    ///     {
    ///        "username": "Sample_User_Name",
    ///        "firstName": "Sample",
    ///        "lastName": "User",
    ///        "email": "sample@user.com",
    ///        "password": "sample_password"
    ///     }
    ///
    /// </remarks>
    /// <response code="201"></response>
    /// <response code="400">if the request data is invalid</response>
    /// <returns>No Content</returns>
    //    [Authorize(Policy = Policies.AdminOnly)]
    [HttpPost("api/register-admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterUserModel request)
    {
        await identityService.RegisterUser(request, UserRoles.Admin);
        return Ok(new {Status = "Success", Message= "User Created Successfully"});
    }


    /// <summary>
    /// login a user
    /// </summary>
    /// <param name="request"></param>
    /// <remarks> 
    /// Sample request:
    ///
    ///     POST /login
    ///     {
    ///        "email": "sample@user.com",
    ///        "password": "sample_password"
    ///     }
    ///
    /// </remarks>
    /// <response code="200"></response>
    /// <response code="400">if the request data is invalid</response>
    /// <returns>userId and a token</returns>

    [HttpPost("api/login")]
    public async Task<ActionResult<LoginOutputModel>> Login([FromBody] LoginUserModel request)
    {
        var result = await identityService.Login(request);
        return Ok(result);
    }
}
