using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.Identity;
using HotelBookingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Application.Tests.IdentityTests;

public class IdentityManagerTests
{
    private readonly IFixture fixture;
    private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> roleManagerMock;
    private readonly Mock<IJwtTokenGenerator> jwtTokenGeneratorMock;
    private readonly IdentityManager sut;

    public IdentityManagerTests()
    {
        userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        fixture = FixtureFactory.CreateFixture();
        sut = new IdentityManager(userManagerMock.Object, roleManagerMock.Object, jwtTokenGeneratorMock.Object);
    }

    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.Guest)]
    public async Task RegisterUser_ShouldCreateUser_WhenDataIsValid(string validRole)
    {
        // Arrange
        var model = fixture.Create<RegisterUserModel>();
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password)).ReturnsAsync(IdentityResult.Success);

        roleManagerMock.Setup(x => x.RoleExistsAsync(validRole)).ReturnsAsync(true);
        userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), validRole)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await sut.RegisterUser(model, validRole);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IUser>(result);
        userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password), Times.Once);
        userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), validRole), Times.Once);
    }


    [Fact]
    public async Task RegisterUser_ShouldCreateNewRoles_WhenRolesDoNotExist()
    {
        // Arrange
        var model = fixture.Create<RegisterUserModel>();
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password)).ReturnsAsync(IdentityResult.Success);
        roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

        // Act
        await sut.RegisterUser(model, UserRoles.Admin);

        // Assert
        roleManagerMock.Verify(x => x.CreateAsync(It.Is<IdentityRole>(r => r.Name == UserRoles.Admin)), Times.Once);
        roleManagerMock.Verify(x => x.CreateAsync(It.Is<IdentityRole>(r => r.Name == UserRoles.Guest)), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldNotCreateRoles_IfAlreadyExists()
    {
        // Arrange
        var model = fixture.Create<RegisterUserModel>();
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password)).ReturnsAsync(IdentityResult.Success);
        roleManagerMock.Setup(x => x.RoleExistsAsync(UserRoles.Admin)).ReturnsAsync(true);
        roleManagerMock.Setup(x => x.RoleExistsAsync(UserRoles.Guest)).ReturnsAsync(true);


        // Act
        await sut.RegisterUser(model, UserRoles.Guest);

        // Assert
        roleManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUser_ShouldThrowBadRequestException_WhenRoleIsInvalid()
    {
        // Arrange
        var model = fixture.Create<RegisterUserModel>();
        var invalidRole = "InvalidRole";

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.RegisterUser(model, invalidRole));
    }


    [Fact]
    public async Task RegisterUser_ShouldThrowBadRequestException_WhenEmailOrUsernameAlreadyExists()
    {
        // Arrange
        var model = fixture.Create<RegisterUserModel>();
        var identityErrors = new List<IdentityError> { new() { Description = "Email already exists" } };
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password)).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => sut.RegisterUser(model, UserRoles.Guest));
        Assert.Contains("Email already exists", exception.Message);
    }




    [Fact]
    public async Task Login_ShouldReturnLoginSuccessModel_WhenCredentialsAreValid()
    {
        // Arrange
        var model = fixture.Create<LoginUserModel>();
        var user = fixture.Create<ApplicationUser>();
        userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);
        userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Role" });
        jwtTokenGeneratorMock.Setup(x => x.GenerateToken(user, It.IsAny<List<string>>())).Returns("token");

        // Act
        var result = await sut.Login(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("token", result.Token);
        Assert.Equal("Role", result.Role);
    }

    [Fact]
    public async Task Login_ShouldThrowInvalidUserCredentialsException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var model = fixture.Create<LoginUserModel>();
        userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync((ApplicationUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidUserCredentialsException>(() => sut.Login(model));
    }

    [Fact]
    public async Task Login_ShouldThrowNoRolesException_WhenNoRolesAssigned()
    {
        // Arrange
        var model = fixture.Create<LoginUserModel>();
        var user = fixture.Create<ApplicationUser>();
        userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);
        userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());

        // Act & Assert
        await Assert.ThrowsAsync<NoRolesException>(() => sut.Login(model));
    }

    [Fact]
    public async Task Login_ShouldThrowTokenGenerationFailedException_WhenTokenIsNull()
    {
        // Arrange
        var model = fixture.Create<LoginUserModel>();
        var user = fixture.Create<ApplicationUser>();
        userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);
        userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Role" });
        jwtTokenGeneratorMock.Setup(x => x.GenerateToken(user, It.IsAny<List<string>>())).Returns((string?)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<TokenGenerationFailedException>(() => sut.Login(model));
    }

}
