using HotelBookingSystem.Application.DTOs.Room;
using HotelBookingSystem.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing rooms
/// </summary>>

[Route("api/[controller]")]
[ApiController]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    /// <summary>
    /// Get all rooms
    /// </summary>
    /// <returns>All rooms</returns>
    /// <response code="200">Returns all rooms</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomOutputModel>>> GetAllRooms()
    {
        var rooms = await roomService.GetAllRoomsAsync();
        return Ok(rooms);
    }

    /// <summary>
    /// Get a room by its id
    /// </summary>
    /// <param name="id">The id of the room</param>
    /// <returns>The room with the given id</returns>
    /// <response code="200">Returns the room with the given id</response>
    /// <response code="404">If the room is not found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomOutputModel>> GetRoom(Guid id)
    {
        var room = await roomService.GetRoomAsync(id);

        return Ok(room);
    }

    /// <summary>
    /// Create a new room
    /// </summary>
    /// <param name="request">The data for the new room</param>
    /// <returns>The newly created room</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /rooms
    ///     {
    ///        "hotelName": "Hotel Budapest",
    ///        "roomNumber": "101",
    ///        "adultsCapacity": 2,
    ///        "childrenCapacity": 1,
    ///        "price": 100,
    ///        "roomType": "Standard"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created room</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<RoomOutputModel>> CreateRoom(CreateRoomCommand request)
    {
        var room = await roomService.CreateRoomAsync(request);

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    /// <summary>
    /// Delete a room
    /// </summary>
    /// <param name="id">The id of the room to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the room is successfully deleted</response>
    /// <response code="404">If the room is not found</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRoom(Guid id)
    {
        var deleted = await roomService.DeleteRoomAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Update a room
    /// </summary>
    /// <param name="id">The id of the room to update</param>
    /// <param name="request">The new data for the room</param>
    /// <returns>No content</returns>
    /// <response code="204">If the room is successfully updated</response>
    /// <response code="404">If the room is not found</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<RoomOutputModel>> UpdateRoom(Guid id, UpdateRoomCommand request)
    {
        var updated = await roomService.UpdateRoomAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}
