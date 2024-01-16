using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Review.Query;

namespace HotelBookingSystem.Application.Tests;

public class ReviewServiceTests
{
    private readonly Mock<IHotelRepository> hotelRepositoryMock;
    private readonly Mock<IGuestRepository> guestRepositoryMock;
    private readonly Mock<IReviewRepository> reviewRepositoryMock;
    private readonly Mock<ICurrentUser> currentUserMock;
    private readonly IFixture fixture;
    private readonly IMapper mapper;
    private readonly Mock<ILogger<ReviewService>> loggerMock;
    private readonly ReviewService sut;

    public ReviewServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        hotelRepositoryMock = new Mock<IHotelRepository>();
        guestRepositoryMock = new Mock<IGuestRepository>();
        reviewRepositoryMock = new Mock<IReviewRepository>();
        currentUserMock = new Mock<ICurrentUser>();
        mapper = AutoMapperSingleton.Mapper;
        loggerMock = new Mock<ILogger<ReviewService>>();

        sut = new ReviewService(hotelRepositoryMock.Object, guestRepositoryMock.Object,
                                reviewRepositoryMock.Object, mapper, currentUserMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task AddReviewAsync_ShouldCreateReview_WhenHotelExistsAndGuestIsAuthorized()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = fixture.Build<Hotel>()
            .With(h => h.Id, hotelId)
            .Create();
        var guest = fixture.Create<Guest>();
        var userId = "user-id";
        var createReviewCommand = fixture.Create<CreateOrUpdateReviewCommand>();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        guestRepositoryMock.Setup(x => x.HasGuestBookedHotelAsync(hotel, guest)).ReturnsAsync(true);
        guestRepositoryMock.Setup(x => x.HasGuestReviewedHotelAsync(hotel, guest)).ReturnsAsync(false);

        // Act
        var result = await sut.AddReviewAsync(hotelId, createReviewCommand);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ReviewOutputModel>(result);
        Assert.Equal(hotelId, result.HotelId);
        reviewRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        reviewRepositoryMock.Verify(r => r.AddReviewAsync(hotel, It.IsAny<Review>()), Times.Once);
    }

    [Fact]
    public async Task AddReviewAsync_ShouldThrowBadRequestException_WhenGuestHasAlreadyReviewedHotel()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = fixture.Create<Hotel>();
        var guest = fixture.Create<Guest>();
        var userId = "user-id";
        var createReviewCommand = fixture.Create<CreateOrUpdateReviewCommand>();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        guestRepositoryMock.Setup(x => x.HasGuestBookedHotelAsync(hotel, guest)).ReturnsAsync(true);
        // the guest has already reviewed the hotel
        guestRepositoryMock.Setup(x => x.HasGuestReviewedHotelAsync(hotel, guest)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.AddReviewAsync(hotelId, createReviewCommand));
    }

    [Fact]
    public async Task AddReviewAsync_ShouldThrowBadRequestException_WhenGuestHasNotBookedTheHotel()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = fixture.Create<Hotel>();
        var guest = fixture.Create<Guest>();
        var userId = "user-id";
        var createReviewCommand = fixture.Create<CreateOrUpdateReviewCommand>();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        guestRepositoryMock.Setup(x => x.HasGuestBookedHotelAsync(hotel, guest)).ReturnsAsync(false);
        // the guest has already reviewed the hotel
        guestRepositoryMock.Setup(x => x.HasGuestReviewedHotelAsync(hotel, guest)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.AddReviewAsync(hotelId, createReviewCommand));
    }

    [Fact]
    public async Task AddReviewAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var createReviewCommand = fixture.Create<CreateOrUpdateReviewCommand>();
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync((Hotel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.AddReviewAsync(hotelId, createReviewCommand));
    }


    [Fact]
    public async Task AddReviewAsync_ShouldThrowUnauthenticatedException_WhenGuestDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = fixture.Create<Hotel>();
        var guest = fixture.Create<Guest>();
        var userId = "user-id";
        var createReviewCommand = fixture.Create<CreateOrUpdateReviewCommand>();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync((Guest?)null);
        currentUserMock.Setup(x => x.Id).Returns(userId);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthenticatedException>(() => sut.AddReviewAsync(hotelId, createReviewCommand));
    }

    [Fact]
    public async Task UpdateReviewAsync_ShouldUpdateReview_WhenGuestIsAuthorized()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();
        var guest = fixture.Create<Guest>();
        var review = fixture.Create<Review>();
        var updateCommand = fixture.Create<CreateOrUpdateReviewCommand>();
        var userId = "user-id";

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(new Hotel());
        reviewRepositoryMock.Setup(x => x.GetReviewAsync(It.IsAny<Hotel>(), reviewId)).ReturnsAsync(review);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        review.GuestId = guest.Id; // To simulate authorized user

        // Act
        var result = await sut.UpdateReviewAsync(hotelId, reviewId, updateCommand);

        // Assert
        Assert.True(result);
        reviewRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateReviewAsync_ShouldThrowNotFoundException_WhenReviewDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();
        var updateCommand = fixture.Create<CreateOrUpdateReviewCommand>();
        var userId = "user-id";

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(new Hotel());
        reviewRepositoryMock.Setup(x => x.GetReviewAsync(It.IsAny<Hotel>(), reviewId)).ReturnsAsync((Review?)null);
        currentUserMock.Setup(x => x.Id).Returns(userId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.UpdateReviewAsync(hotelId, reviewId, updateCommand));
    }

    [Fact]
    public async Task UpdateReviewAsync_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();
        var guest = fixture.Create<Guest>();
        var review = fixture.Create<Review>();
        var updateCommand = fixture.Create<CreateOrUpdateReviewCommand>();
        var userId = "user-id";

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(new Hotel());
        reviewRepositoryMock.Setup(x => x.GetReviewAsync(It.IsAny<Hotel>(), reviewId)).ReturnsAsync(review);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        review.GuestId = Guid.NewGuid(); // To simulate unauthorized user

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.UpdateReviewAsync(hotelId, reviewId, updateCommand));
    }

    [Fact]
    public async Task UpdateReviewAsync_ShouldThrowUnauthenticatedException_WhenGuestDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();
        var guest = fixture.Create<Guest>();
        var review = fixture.Create<Review>();
        var updateCommand = fixture.Create<CreateOrUpdateReviewCommand>();
        var userId = "user-id";

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(new Hotel());
        reviewRepositoryMock.Setup(x => x.GetReviewAsync(It.IsAny<Hotel>(), reviewId)).ReturnsAsync(review);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync((Guest?)null);


        // Act & Assert
        await Assert.ThrowsAsync<UnauthenticatedException>(() => sut.AddReviewAsync(hotelId, updateCommand));
    }

    [Fact]
    public async Task GetReviewAsync_ShouldReturnReview_WhenReviewExists()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();
        var hotel = new Hotel();
        var review = new Review();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        reviewRepositoryMock.Setup(x => x.GetReviewAsync(hotel, reviewId)).ReturnsAsync(review);

        // Act
        var result = await sut.GetReviewAsync(hotelId, reviewId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ReviewOutputModel>(result);
    }

    [Fact]
    public async Task GetReviewAsync_ShouldThrowNotFoundException_WhenReviewDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(new Hotel());
        reviewRepositoryMock.Setup(x => x.GetReviewAsync(It.IsAny<Hotel>(), reviewId)).ReturnsAsync((Review?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetReviewAsync(hotelId, reviewId));
    }

    [Fact]
    public async Task GetReviewAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync((Hotel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetReviewAsync(hotelId, reviewId));
    }

    [Fact]
    public async Task GetHotelAverageRatingAsync_ShouldReturnRoundedAverageRating_WhenHotelExists()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var expectedRating = 4.6;
        var roundedExpectedRating = Math.Round(expectedRating, 1);
        var hotel = new Hotel();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        reviewRepositoryMock.Setup(x => x.GetHotelAverageRatingAsync(hotel)).ReturnsAsync(expectedRating);

        // Act
        var result = await sut.GetHotelAverageRatingAsync(hotelId);

        // Assert
        Assert.Equal(roundedExpectedRating, result);
    }

    [Fact]
    public async Task GetHotelAverageRatingAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync((Hotel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetHotelAverageRatingAsync(hotelId));
    }

    [Fact]
    public async Task GetHotelReviewsAsync_ShouldReturnReviewsWithCorrectPagination_WhenHotelExists()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var request = new GetHotelReviewsQueryParameters { PageNumber = 1, PageSize = 5 };

        var hotel = new Hotel();
        var reviews = fixture.CreateMany<Review>(5);
        var expectedPaginationMetadata = new PaginationMetadata(1, 5, 5); // Assuming 1 page, 5 items per page, 5 total items

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(hotel);
        reviewRepositoryMock.Setup(x => x.GetHotelReviewsAsync(hotel, request)).ReturnsAsync((reviews, expectedPaginationMetadata));

        // Act
        var (resultReviews, paginationMetadata) = await sut.GetHotelReviewsAsync(hotelId, request);

        // Assert
        Assert.Equal(reviews.Count(), resultReviews.Count());
        Assert.Equal(expectedPaginationMetadata, paginationMetadata);
        reviewRepositoryMock.Verify(x => x.GetHotelReviewsAsync(hotel, request), Times.Once);
    }

    [Fact]
    public async Task GetHotelReviewsAsync_ShouldReturnReviewsWithCorrectPaginationMetadata_WhenReviewsCountLessThanOrEqualPageSize()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var requestParameters = new GetHotelReviewsQueryParameters { PageNumber = 1, PageSize = 5 };
        var expectedReviews = fixture.CreateMany<Review>(5).ToList();
        var expectedPaginationMetadata = new PaginationMetadata(1, 5, 5); //page 1, 5 items per page, 5 total items

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(fixture.Create<Hotel>());
        reviewRepositoryMock.Setup(x => x.GetHotelReviewsAsync(It.IsAny<Hotel>(), requestParameters))
                            .ReturnsAsync((expectedReviews, expectedPaginationMetadata));

        // Act
        var (reviews, paginationMetadata) = await sut.GetHotelReviewsAsync(hotelId, requestParameters);

        // Assert
        reviewRepositoryMock.Verify(r => r.GetHotelReviewsAsync(It.IsAny<Hotel>(), requestParameters), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<ReviewOutputModel>>(reviews);
        Assert.Equal(expectedReviews.Count, reviews.Count());

        Assert.IsType<PaginationMetadata>(paginationMetadata);
        Assert.Equal(expectedPaginationMetadata.PageNumber, paginationMetadata.PageNumber);
        Assert.Equal(expectedPaginationMetadata.PageSize, paginationMetadata.PageSize);
        Assert.Equal(expectedPaginationMetadata.TotalCount, paginationMetadata.TotalCount);
        Assert.False(paginationMetadata.HasPreviousPage);
        Assert.False(paginationMetadata.HasNextPage);
    }

    [Fact]
    public async Task GetHotelReviewsAsync_ShouldHandlePaginationCorrectly()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var requestParameters = new GetHotelReviewsQueryParameters { PageNumber = 2, PageSize = 5 };
        var expectedReviews = fixture.CreateMany<Review>(10).ToList();
        var expectedPaginationMetadata = new PaginationMetadata(2, 5, 10); // page 2, 5 items per page, 10 total items

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync(fixture.Create<Hotel>());
        reviewRepositoryMock.Setup(x => x.GetHotelReviewsAsync(It.IsAny<Hotel>(), requestParameters))
                            .ReturnsAsync((expectedReviews.Skip(5).Take(5).ToList(), expectedPaginationMetadata));

        // Act
        var (reviews, paginationMetadata) = await sut.GetHotelReviewsAsync(hotelId, requestParameters);

        // Assert
        Assert.Equal(5, reviews.Count());
        Assert.Equal(2, paginationMetadata.PageNumber);
        Assert.True(paginationMetadata.HasPreviousPage);
        Assert.False(paginationMetadata.HasNextPage);
    }



    [Fact]
    public async Task GetHotelReviewsAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var request = new GetHotelReviewsQueryParameters();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotelId)).ReturnsAsync((Hotel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetHotelReviewsAsync(hotelId, request));
    }



}
