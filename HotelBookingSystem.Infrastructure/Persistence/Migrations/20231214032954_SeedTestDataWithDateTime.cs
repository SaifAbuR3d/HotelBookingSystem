using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations;

/// <inheritdoc />
public partial class SeedTestDataWithDateTime : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(14, 0, 0), new TimeOnly(12, 0, 0), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(15, 0, 0), new TimeOnly(11, 30, 0), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(14, 30, 0), new TimeOnly(12, 30, 0), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(14, 0, 0), new TimeOnly(12, 0, 0), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(15, 0, 0), new TimeOnly(11, 30, 0), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3283b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3383b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3483b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3583b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2583b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Bookings",
            keyColumn: "Id",
            keyValue: new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Cities",
            keyColumn: "Id",
            keyValue: new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Guests",
            keyColumn: "Id",
            keyValue: new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Hotels",
            keyColumn: "Id",
            keyValue: new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CheckInTime", "CheckOutTime", "CreationDate", "LastModified" },
            values: new object[] { new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3283b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3383b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3483b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Reviews",
            keyColumn: "Id",
            keyValue: new Guid("3583b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2583b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "Rooms",
            keyColumn: "Id",
            keyValue: new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e"),
            columns: new[] { "CreationDate", "LastModified" },
            values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
    }
}
