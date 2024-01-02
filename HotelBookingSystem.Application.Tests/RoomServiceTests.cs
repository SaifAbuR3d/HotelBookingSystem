using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Room.Command;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Application.DTOs.Room.Query;
using HotelBookingSystem.Application.Tests.Shared;

namespace HotelBookingSystem.Application.Tests;

public class RoomServiceTests
{
    private readonly Mock<IHotelRepository> hotelRepositoryMock;
    private readonly Mock<IRoomRepository> roomRepositoryMock;
    private readonly IFixture fixture;
    private readonly IMapper mapper;
    private readonly RoomService sut;

    public RoomServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        hotelRepositoryMock = new Mock<IHotelRepository>();
        roomRepositoryMock = new Mock<IRoomRepository>();
        mapper = AutoMapperSingleton.Mapper;
        var imageHandler = new Mock<IImageHandler>();

        sut = new RoomService(hotelRepositoryMock.Object,
                              roomRepositoryMock.Object,
                              mapper,
                              imageHandler.Object);
    }

    [Fact]
    public async Task GetAllRoomsAsync_ShouldReturnAllRoomsWithCorrectPaginationMetadata_IfRoomsCountIsLessThanOrEqualPageSize()
    {
        // Arrange
        var expectedRooms = fixture.CreateMany<Room>(10);
        var parameters = new GetRoomsQueryParameters();
        var expectedPaginationMetadata = new PaginationMetadata(1, 10, 10); //page 1, 10 items per page, 10 total items

        roomRepositoryMock.Setup(x => x.GetAllRoomsAsync(parameters)).ReturnsAsync((expectedRooms, expectedPaginationMetadata));

        // Act
        var (rooms, paginationMetadata) = await sut.GetAllRoomsAsync(parameters);

        // Assert
        roomRepositoryMock.Verify(c => c.GetAllRoomsAsync(parameters), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<RoomOutputModel>>(rooms);
        Assert.Equal(expectedRooms.Count(), rooms.Count());

        Assert.IsType<PaginationMetadata>(paginationMetadata);
        Assert.Equal(expectedPaginationMetadata.PageNumber, paginationMetadata.PageNumber);
        Assert.Equal(expectedPaginationMetadata.PageSize, paginationMetadata.PageSize);
        Assert.Equal(expectedPaginationMetadata.TotalCount, paginationMetadata.TotalCount);
        Assert.False(paginationMetadata.HasPreviousPage);
        Assert.False(paginationMetadata.HasNextPage);
    }

    [Fact]
    public async Task GetRoomAsync_ShouldReturnRoom_IfRoomExists()
    {
        // Arrange
        var room = fixture.Create<Room>();
        roomRepositoryMock.Setup(x => x.GetRoomAsync(room.Id)).ReturnsAsync(room);

        // Act
        var result = await sut.GetRoomAsync(room.Id);

        // Assert
        roomRepositoryMock.Verify(r => r.GetRoomAsync(room.Id), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<RoomOutputModel>(result);
        Assert.Equal(room.RoomNumber, result.RoomNumber);
    }

    [Fact]
    public async Task GetRoomAsync_ShouldThrowNotFoundException_IfRoomDoesNotExist()
    {
        // Arrange
        roomRepositoryMock.Setup(x => x.GetRoomAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);

        // Act & Assert
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.GetRoomAsync(It.IsAny<Guid>()));
        roomRepositoryMock.Verify(r => r.GetRoomAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldCallSaveChangesAndReturnTrue_IfRoomExists()
    {
        // Arrange
        roomRepositoryMock.Setup(x => x.DeleteRoomAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await sut.DeleteRoomAsync(It.IsAny<Guid>());

        // Assert
        roomRepositoryMock.Verify(c => c.DeleteRoomAsync(It.IsAny<Guid>()), Times.Once);
        roomRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldNotCallSaveChangesAndReturnFalse_IfRoomDoesNotExist()
    {
        // Arrange
        roomRepositoryMock.Setup(x => x.DeleteRoomAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await sut.DeleteRoomAsync(It.IsAny<Guid>());

        // Assert
        roomRepositoryMock.Verify(r => r.DeleteRoomAsync(It.IsAny<Guid>()), Times.Once);
        hotelRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Never);
        Assert.False(result);
    }

    [Fact]
    public async Task CreateRoomAsync_ShouldReturnCreatedRoom_IfHotelNameExists()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();
        var room = fixture.Create<Room>();
        var createRoomCommand = fixture.Build<CreateRoomCommand>()
            .With(x => x.HotelName, hotel.Name)
            .Create();

        hotelRepositoryMock.Setup(x => x.GetHotelByNameAsync(createRoomCommand.HotelName)).ReturnsAsync(hotel);
        roomRepositoryMock.Setup(x => x.AddRoomAsync(It.IsAny<Room>())).ReturnsAsync(room);

        // Act
        var result = await sut.CreateRoomAsync(createRoomCommand);

        // Assert
        hotelRepositoryMock.Verify(h => h.GetHotelByNameAsync(createRoomCommand.HotelName), Times.Once);
        roomRepositoryMock.Verify(r => r.AddRoomAsync(It.IsAny<Room>()), Times.Once);
        roomRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<RoomOutputModel>(result);
        Assert.Equal(room.RoomNumber, result.RoomNumber);
    }

    [Fact]
    public async Task CreateRoomAsync_ShouldThrowNotFoundException_IfHotelNameDoesNotExist()
    {
        // Arrange
        var createRoomCommand = fixture.Create<CreateRoomCommand>();
        hotelRepositoryMock.Setup(x => x.GetHotelByNameAsync(createRoomCommand.HotelName)).ReturnsAsync((Hotel?)null);

        // Act & Assert
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.CreateRoomAsync(createRoomCommand));
        hotelRepositoryMock.Verify(h => h.GetHotelByNameAsync(createRoomCommand.HotelName), Times.Once);
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldCallSaveChangesAndReturnTrue_IfRoomExists()
    {
        // Arrange
        var updateRoomCommand = fixture.Create<UpdateRoomCommand>();
        var room = mapper.Map<Room>(updateRoomCommand);
        roomRepositoryMock.Setup(x => x.GetRoomAsync(It.IsAny<Guid>())).ReturnsAsync(room);

        // Act
        var result = await sut.UpdateRoomAsync(It.IsAny<Guid>(), updateRoomCommand);

        // Assert
        roomRepositoryMock.Verify(c => c.GetRoomAsync(It.IsAny<Guid>()), Times.Once);
        roomRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldThrowNotFoundException_IfRoomDoesNotExist()
    {
        // Arrange
        roomRepositoryMock.Setup(x => x.GetRoomAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);

        // Act & Asser
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.UpdateRoomAsync(It.IsAny<Guid>(), It.IsAny<UpdateRoomCommand>()));
        Assert.NotNull(result);
    }


}
