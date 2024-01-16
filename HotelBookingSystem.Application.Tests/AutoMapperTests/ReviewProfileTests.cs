using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class ReviewProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public ReviewProfileTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
    }


    [Fact]
    public void ReviewToReviewOutputModelMappingTest()
    {
        // Arrange
        var review = fixture.Create<Review>();

        // Act
        var reviewOutputModel = mapper.Map<ReviewOutputModel>(review);

        // Assert
        Assert.Equal(review.Id, reviewOutputModel.Id);
        Assert.Equal(review.Title, reviewOutputModel.Title);
        Assert.Equal(review.Description, reviewOutputModel.Description);
        Assert.Equal(review.Rating, reviewOutputModel.Rating);
        Assert.Equal(review.CreationDate, reviewOutputModel.CreationDate);
        Assert.Equal(review.LastModified, reviewOutputModel.LastModified);
        Assert.Equal(review.Guest.Id, reviewOutputModel.Guest.Id);
        Assert.Equal(review.HotelId, reviewOutputModel.HotelId);
        Assert.Equal(review.Hotel.Name, reviewOutputModel.HotelName);
    }

    [Fact]
    public void CreateOrUpdateReviewCommandToReviewMappingTest()
    {
        // Arrange
        var command = fixture.Create<CreateOrUpdateReviewCommand>();

        // Act
        var review = mapper.Map<Review>(command);

        // Assert
        Assert.Equal(command.Title, review.Title);
        Assert.Equal(command.Description, review.Description);
        Assert.Equal(command.Rating, review.Rating);
    }

}
