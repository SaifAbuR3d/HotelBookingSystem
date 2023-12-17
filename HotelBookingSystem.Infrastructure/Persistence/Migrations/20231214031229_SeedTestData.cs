using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBookingSystem.Infrastructure.Migrations;

/// <inheritdoc />
public partial class SeedTestData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Cities",
            columns: new[] { "Id", "Country", "CreationDate", "LastModified", "Name", "PostOffice" },
            values: new object[,]
            {
                { new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"), "USA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New York", "10001" },
                { new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"), "UK", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "London", "SW1A 1AA" },
                { new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"), "France", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paris", "75001" },
                { new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"), "Japan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tokyo", "100-0001" },
                { new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"), "Germany", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Berlin", "10115" }
            });

        migrationBuilder.InsertData(
            table: "Guests",
            columns: new[] { "Id", "CreationDate", "LastModified", "Username" },
            values: new object[,]
            {
                { new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john_doe" },
                { new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane_smith" },
                { new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice_jones" },
                { new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob_smith" }
            });

        migrationBuilder.InsertData(
            table: "Hotels",
            columns: new[] { "Id", "CheckInTime", "CheckOutTime", "CityId", "CreationDate", "Description", "LastModified", "Location", "Name", "Owner", "StarRate" },
            values: new object[,]
            {
                { new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"), new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Times Square, New York", "Grand Hyatt", "Hyatt Group", (short)5 },
                { new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"), new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piccadilly, London", "The Ritz", "Ritz-Carlton", (short)5 },
                { new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"), new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Champs-Élysées, Paris", "Le Méridien", "Marriott Group", (short)4 },
                { new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"), new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chiyoda, Tokyo", "Tokyo Palace", "Palace Hotels", (short)4 },
                { new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"), new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mitte, Berlin", "Berlin Grand", "Grand Hotels", (short)4 }
            });

        migrationBuilder.InsertData(
            table: "Reviews",
            columns: new[] { "Id", "CreationDate", "Description", "GuestId", "HotelId", "LastModified", "Rating", "Title" },
            values: new object[,]
            {
                { new Guid("3283b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wonderful experience at Grand Hyatt!", new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"), new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Excellent Stay" },
                { new Guid("3383b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Ritz never disappoints.", new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"), new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)4, "Amazing Service" },
                { new Guid("3483b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Great service and comfortable stay.", new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"), new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Wonderful Experience" },
                { new Guid("3583b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Enjoyed my time at Berlin Grand.", new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"), new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)4, "Good Stay" }
            });

        migrationBuilder.InsertData(
            table: "Rooms",
            columns: new[] { "Id", "AdultsCapacity", "ChildrenCapacity", "CreationDate", "Description", "HotelId", "LastModified", "Price", "RoomNumber", "RoomType" },
            values: new object[,]
            {
                { new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e"), 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 300m, 1, 0 },
                { new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e"), 3, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, 1, 1 },
                { new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e"), 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 250m, 1, 3 },
                { new Guid("2583b59c-f7f8-4b21-b1df-5149fb57984e"), 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 350m, 1, 0 },
                { new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e"), 2, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 450m, 1, 3 }
            });

        migrationBuilder.InsertData(
            table: "Bookings",
            columns: new[] { "Id", "CheckInDate", "CheckOutDate", "CreationDate", "GuestId", "LastModified", "Price", "RoomId" },
            values: new object[,]
            {
                { new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 3, 5), new DateOnly(2023, 3, 10), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 600m, new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e") },
                { new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 4, 15), new DateOnly(2023, 4, 20), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 700m, new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e") },
                { new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 5, 8), new DateOnly(2023, 5, 15), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 550m, new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e") },
                { new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 6, 20), new DateOnly(2023, 6, 25), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 800m, new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e") }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3283b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3383b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3483b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3583b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2583b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"));

        migrationBuilder.DeleteData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"));
    }
}
