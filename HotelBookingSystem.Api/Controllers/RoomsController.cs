using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Room.Command;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Application.DTOs.Room.Query;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing rooms
/// </summary>>

[Route("api/[controller]")]
[ApiController]
public class RoomsController(IRoomService roomService, IWebHostEnvironment environment) : ControllerBase
{

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

    /// <summary>
    /// Upload an image to a room
    /// </summary>
    /// <param name="id">The id of the room to upload image</param>
    /// <param name="file">HotelImage data</param>
    /// <param name="alternativeText">Alternative Text(Alt)</param>
    /// <param name="thumbnail">indicates if the image should be used as thumbnail</param>
    /// <returns></returns>
    /// <response code="204">If the image is successfully uploaded</response>
    /// <response code="404">If the room is not found</response>
    /// <response code="400">If the request data is invalid</response>

    [HttpPost("{id}/images")]
    public async Task<ActionResult> UploadImage(Guid id, IFormFile file, string? alternativeText, bool? thumbnail = false)
    {
        var uploaded = await roomService.UploadImageAsync(id, file, environment.WebRootPath, alternativeText, thumbnail);

        if (!uploaded)
        {
            return NotFound();
        }

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
    [HttpGet(Name = "GetRooms")]
    public async Task<ActionResult<IEnumerable<RoomOutputModel>>> GetRooms([FromQuery] GetRoomsQueryParameters request)
    {
        var (rooms, paginationMetadata) = await roomService.GetAllRoomsAsync(request);

        AddPageLinks(paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(rooms);
    }

    private void AddPageLinks(PaginationMetadata paginationMetadata, ResourceQueryParameters parameters)
    {
        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ? CreatePageLink(paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ? CreatePageLink(paginationMetadata, parameters, next: true) : null;
    }

    private string? CreatePageLink(PaginationMetadata paginationMetadata, ResourceQueryParameters parameters, bool next)
    {
        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        return
            Url.Link("GetRooms", new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,
            });
    }
}
