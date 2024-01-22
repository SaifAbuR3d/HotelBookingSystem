using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Globalization;

namespace HotelBookingSystem.Infrastructure.PDF;

public class PdfGenerator : IPdfGenerator
{

    public byte[] GeneratePdf(Invoice invoice)
    {
        var cultureInfo = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);

                page.Header()
                    .Text("Invoice")
                    .FontSize(20)
                    .Bold()
                    .FontColor(Colors.Blue.Medium);

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(15);

                        column.Item().Text($"Confirmation ID: {invoice.ConfirmationId}").SemiBold();
                        column.Item().Text($"Guest Name: {invoice.GuestFullName}");
                        column.Item().Text($"Check-In Date: {invoice.CheckInDate}");
                        column.Item().Text($"Check-Out Date: {invoice.CheckOutDate}");
                        column.Item().Text($"Hotel: {invoice.Hotel.Name}");
                        column.Item().Text($"Hotel Location: {invoice.Hotel.CityName}, {invoice.Hotel.Street}");

                        column.Item()
                            .Text("Rooms")
                            .FontSize(18)
                            .Bold();


                        foreach (var room in invoice.Rooms)
                        {
                            column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5)
                                .Column(roomColumn =>
                                {
                                    roomColumn.Item().Text($"Room Number: {room.RoomNumber}, Type: {room.RoomType}");
                                    roomColumn.Item().Text($"Adults Capacity: {room.AdultsCapacity}, Children Capacity: {room.ChildrenCapacity}");
                                    roomColumn.Item().Text($"Price Per Night: {room.PricePerNight:C}, Nights: {room.NumberOfNights}");
                                    roomColumn.Item().Text($"Total Room Price: {room.TotalRoomPrice:C}");
                                    roomColumn.Item().Text($"Total Room Price After Discount: {room.TotalRoomPriceAfterDiscount:C}");
                                });
                        }

                        column.Item()
                            .Text($"Total Price: {invoice.TotalPrice:C}")
                            .FontSize(16)
                            .Bold();

                        column.Item()
                            .Text($"Total Price After Discount: {invoice.TotalPriceAfterDiscount:C}")
                            .FontSize(16)
                            .Bold();
                    });

                page.Footer()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.CurrentPageNumber().FontColor(Colors.Blue.Medium);
                    });
            });
        });


        return document.GeneratePdf(); 
    }

}