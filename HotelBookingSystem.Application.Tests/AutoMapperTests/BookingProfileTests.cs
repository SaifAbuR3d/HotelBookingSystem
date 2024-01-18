using HotelBookingSystem.Application.DTOs.Booking.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class BookingProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;
    public BookingProfileTests()
    {
        mapper = AutoMapperSingleton.Mapper;
        fixture = FixtureFactory.CreateFixture();
    }

    [Fact]
    public void BookingToBookingConfirmationOutputModelMapping()
    {
        // Arrange
        var guest = fixture.Create<Guest>();
        var hotel = fixture.Create<Hotel>();
        var rooms = fixture.CreateMany<Room>().ToList();
        var booking = fixture.Build<Booking>()
            .With(b => b.Guest, guest)
            .With(b => b.Hotel, hotel)
            .With(b => b.Rooms, rooms)
            .With(b => b.GuestId, guest.Id)
            .With(b => b.HotelId, hotel.Id)
            .Create();


        // Act
        var bookingConfirmation = mapper.Map<BookingOutputModel>(booking);

        // Assert
        Assert.NotNull(bookingConfirmation);
        Assert.IsType<BookingOutputModel>(bookingConfirmation);
        Assert.Equal(booking.Id, bookingConfirmation.ConfirmationId);
        Assert.Equal(booking.CheckInDate, bookingConfirmation.CheckInDate);
        Assert.Equal(booking.Guest.FullName, bookingConfirmation.GuestFullName);
        Assert.Equal(booking.Hotel.Name, bookingConfirmation.HotelName);
        Assert.Equal(booking.Rooms.Select(r => r.RoomNumber), bookingConfirmation.RoomNumbers);
        Assert.Equal(booking.GuestId, bookingConfirmation.GuestId);
        Assert.Equal(booking.HotelId, bookingConfirmation.HotelId);
        Assert.Equal(booking.CheckOutDate, bookingConfirmation.CheckOutDate);
        Assert.Equal(booking.NumberOfAdults, bookingConfirmation.NumberOfAdults);
        Assert.Equal(booking.NumberOfChildren, bookingConfirmation.NumberOfChildren);
        Assert.Equal(booking.Price, bookingConfirmation.Price);
    }

    /// <summary>
    /// Note: TotalPriceAfterDiscount and RoomsWithinInvoice are ignored in the mapping,
    /// because they are calculated at The service when needed. So they are not checked here.
    /// the logic of calculating them is tested in the <see cref="BookingTests.BookingServiceInvoiceCalculationsTests"/> 
    /// </summary>
    [Fact]
    public void BookingToInvoiceMapping()
    {
        // Arrange
        var guest = fixture.Create<Guest>();
        var hotel = fixture.Create<Hotel>();
        var rooms = fixture.CreateMany<Room>().ToList();
        var booking = fixture.Build<Booking>()
            .With(b => b.Guest, guest)
            .With(b => b.Hotel, hotel)
            .With(b => b.Rooms, rooms)
            .With(b => b.GuestId, guest.Id)
            .With(b => b.HotelId, hotel.Id)
            .Create();

        // Act
        var invoice = mapper.Map<Invoice>(booking);

        // Assert
        Assert.NotNull(invoice);
        Assert.IsType<Invoice>(invoice);
        Assert.Equal(booking.Id, invoice.ConfirmationId);
        Assert.Equal(booking.GuestId, invoice.GuestId);
        Assert.Equal(booking.Guest.FullName, invoice.GuestFullName);
        Assert.Equal(booking.CheckInDate, invoice.CheckInDate);
        Assert.Equal(booking.CheckOutDate, invoice.CheckOutDate);
        Assert.Equal(booking.NumberOfAdults, invoice.NumberOfAdults);
        Assert.Equal(booking.NumberOfChildren, invoice.NumberOfChildren);
        Assert.Equal(booking.Price, invoice.TotalPrice);
        Assert.Equal(booking.Hotel.Name, invoice.Hotel.Name);
        Assert.Equal(booking.HotelId, invoice.Hotel.Id);
    }

}
