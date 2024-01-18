using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.IdentityInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.DTOs.Identity.OutputModel;
using HotelBookingSystem.Application.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Application.Tests.IdentityTests;

public class IdentityServiceTests
{
    private readonly IFixture fixture;
    private readonly Mock<IIdentityManager> identityManagerMock;
    private readonly Mock<IGuestRepository> guestRepositoryMock;
    private readonly IdentityService sut;

    public IdentityServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        identityManagerMock = new Mock<IIdentityManager>();
        guestRepositoryMock = new Mock<IGuestRepository>();
        sut = new IdentityService(identityManagerMock.Object, guestRepositoryMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnTokenForAdmin_WhenSuccessful()
    {
        // Arrange
        var loginUserModel = new LoginUserModel("admin@example.com", "password123");
        var userId = "user1";
        var validToken = "ValidToken";
        var loginSuccessModel = new LoginSuccessModel(userId, validToken, UserRoles.Admin);

        identityManagerMock.Setup(x => x.Login(loginUserModel)).ReturnsAsync(loginSuccessModel);

        // Act
        var result = await sut.Login(loginUserModel);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<LoginOutputModel>(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(validToken, result.Token);
    }

    [Fact]
    public async Task Login_ShouldReturnTokenForGuest_WhenSuccessful()
    {
        // Arrange
        var loginUserModel = new LoginUserModel("guest@example.com", "password123");
        var userId = "user1";
        var validToken = "ValidToken";
        var loginSuccessModel = new LoginSuccessModel(userId, validToken, UserRoles.Guest);
        var guest = fixture.Create<Guest>();

        identityManagerMock.Setup(x => x.Login(loginUserModel)).ReturnsAsync(loginSuccessModel);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync(guest);

        // Act
        var result = await sut.Login(loginUserModel);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<LoginOutputModel>(result);
        Assert.Equal(guest.Id.ToString(), result.UserId);
        Assert.Equal("ValidToken", result.Token);
    }

    [Fact]
    public async Task Login_ShouldThrowNotFoundException_WhenGuestNotFound()
    {
        // Arrange
        var loginUserModel = new LoginUserModel("guest@example.com", "password123");
        var userId = "user1";
        var validToken = "ValidToken";
        var loginSuccessModel = new LoginSuccessModel(userId, validToken, UserRoles.Guest);

        identityManagerMock.Setup(x => x.Login(loginUserModel)).ReturnsAsync(loginSuccessModel);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync((Guest?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.Login(loginUserModel));
    }

    [Fact]
    public async Task RegisterUser_ShouldRegisterAdmin_WhenRoleIsAdmin()
    {
        // Arrange
        var registerUserModel = fixture.Create<RegisterUserModel>();
        var userMock = new Mock<IUser>();

        identityManagerMock.Setup(x => x.RegisterUser(registerUserModel, UserRoles.Admin))
                           .ReturnsAsync(userMock.Object);

        // Act
        var result = await sut.RegisterUser(registerUserModel, UserRoles.Admin);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IUser>(result);
        identityManagerMock.Verify(x => x.RegisterUser(registerUserModel, UserRoles.Admin), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldRegisterGuest_WhenRoleIsGuest()
    {
        // Arrange
        var registerUserModel = fixture.Create<RegisterUserModel>();
        var userMock = new Mock<IUser>();
        var guest = new Guest(registerUserModel.FirstName, registerUserModel.LastName);

        identityManagerMock.Setup(x => x.RegisterUser(registerUserModel, UserRoles.Guest))
                           .ReturnsAsync(userMock.Object);
        guestRepositoryMock.Setup(x => x.AddGuestAsync(It.IsAny<Guest>())).ReturnsAsync(guest);

        // Act
        var result = await sut.RegisterUser(registerUserModel, UserRoles.Guest);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IUser>(result);
        userMock.Verify(x => x.BecomeGuest(It.IsAny<Guest>()), Times.Once);
        guestRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        guestRepositoryMock.Verify(x => x.AddGuestAsync(It.IsAny<Guest>()), Times.Once);
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("ADMIN")]
    [InlineData("guest")]
    [InlineData("GUEST")]
    [InlineData("invalid role")]

    public async Task RegisterUser_ShouldThrowBadRequestException_WhenRoleIsInvalid(string invalidRole)
    {
        // Arrange
        var registerUserModel = fixture.Create<RegisterUserModel>();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.RegisterUser(registerUserModel, invalidRole));
    }


}
