﻿using Asp.Versioning;
using HotelBookingSystem.Api.Helpers;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Room.Command;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Application.DTOs.Room.Query;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing rooms
/// </summary>>

[ApiVersion("1.0")]
[Authorize(Policy = Policies.AdminOnly)]
[Route("api/[controller]")]
[ApiController]
public class RoomsController(IRoomService roomService,
                             IWebHostEnvironment environment, 
                             ILogger<RoomsController> logger) : ControllerBase
{

    /// <summary>
    /// Get a room by its id
    /// </summary>
    /// <param name="id">The id of the room</param>
    /// <returns>The room with the given id</returns>
    /// <response code="200">Returns the room with the given id</response>
    /// <response code="404">If the room is not found</response>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomOutputModel>> GetRoom(Guid id)
    {
        logger.LogInformation("GetRoom started for room with ID: {RoomId}", id);

        var room = await roomService.GetRoomAsync(id);

        logger.LogInformation("GetRoom for room with ID: {RoomId} completed successfully", id);
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
    ///        "hotelId": "{hotelId}",
    ///        "roomNumber": "101",
    ///        "adultsCapacity": 2,
    ///        "childrenCapacity": 1,
    ///        "price": 100,
    ///        "roomType": "Standard"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created room</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<RoomOutputModel>> CreateRoom(CreateRoomCommand request)
    {
        logger.LogInformation("CreateRoom started for request: {@CreateRoom}", request);

        var room = await roomService.CreateRoomAsync(request);

        logger.LogInformation("CreateRoom for request: {@CreateRoom} completed successfully", request);
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    /// <summary>
    /// Delete a room
    /// </summary>
    /// <param name="id">The id of the room to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the operation is successfully done</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRoom(Guid id)
    {
        logger.LogInformation("DeleteRoom started for room with ID: {RoomId}", id);

        await roomService.DeleteRoomAsync(id);

        logger.LogInformation("DeleteRoom for room with ID: {RoomId} completed successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Update a room
    /// </summary>
    /// <param name="id">The id of the room to update</param>
    /// <param name="request">The new data for the room</param>
    /// <returns>No content</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /rooms/{roomId}
    ///     {
    ///        "roomNumber": "101",
    ///        "adultsCapacity": 2,
    ///        "childrenCapacity": 1,
    ///        "price": 100
    ///     }
    ///
    /// </remarks> 
    /// <response code="204">If the room is successfully updated</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    /// <response code="404">If the room is not found</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<RoomOutputModel>> UpdateRoom(Guid id, UpdateRoomCommand request)
    {
        logger.LogInformation("UpdateRoom started for room with ID: {RoomId} and request: {@UpdateRoom}", id, request);

        await roomService.UpdateRoomAsync(id, request);

        logger.LogInformation("UpdateRoom for room with ID: {RoomId} and request: {@UpdateRoom} completed successfully", id, request);
        return NoContent();
    }

    /// <summary>
    /// Upload an image to a room
    /// </summary>
    /// <param name="id">The id of the room to upload image</param>
    /// <param name="file">HotelImage data</param>
    /// <param name="alternativeText">Alternative Text(Alt)</param>
    /// <param name="thumbnail">indicates if the image should be used as thumbnail</param>
    /// <returns>No content</returns>
    /// <response code="204">If the image is successfully uploaded</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    /// <response code="404">If the room is not found</response>
    [HttpPost("{id}/images")]
    public async Task<ActionResult> UploadImage(Guid id, IFormFile file, string? alternativeText, bool? thumbnail = false)
    {
        logger.LogInformation("UploadImage started for room with ID: {RoomId}", id);

        await roomService.UploadImageAsync(id, file, environment.WebRootPath, alternativeText, thumbnail);

        logger.LogInformation("UploadImage for room with ID: {RoomId} completed successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of rooms based on the specified query parameters.
    /// </summary>
    /// <remarks>
    /// The retrieval of rooms can be customized by providing various query parameters.
    /// These parameters include sorting options, page number, page size, and a search term.
    /// 
    /// Sample request:
    /// 
    ///     GET /rooms?sortOrder=asc&amp;sortColumn=price&amp;pageNumber=1&amp;pageSize=10&amp;searchQuery=Ritz
    ///     
    /// </remarks>
    /// <param name="request">The query parameters for room retrieval.</param>
    /// <returns>
    /// A collection of <see cref="RoomOutputModel"/> objects, each representing a room that matches the specified criteria.
    /// </returns>
    /// <response code="200">Returns the list of rooms based on the query parameters.</response>
    /// <response code="400">If the request parameters are invalid or missing.</response>
    [AllowAnonymous]
    [HttpGet(Name = "GetRooms")]
    public async Task<ActionResult<IEnumerable<RoomOutputModel>>> GetRooms([FromQuery] GetRoomsQueryParameters request)
    {
        logger.LogInformation("GetRooms started for query: {@GetRoomsQuery}", request);

        var (rooms, paginationMetadata) = await roomService.GetAllRoomsAsync(request);

        PageLinker.AddPageLinks(Url, nameof(GetRoom), paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        logger.LogInformation("GetRooms for query: {@GetRoomsQuery} completed successfully", request);
        return Ok(rooms);
    }
}
