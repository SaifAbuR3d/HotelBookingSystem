using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomFromBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

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


            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CheckInDate", "CheckOutDate", "CreationDate", "GuestId", "LastModified", "NumberOfAdults", "NumberOfChildren", "Price", "RoomId" },
                values: new object[,]
                {
                    { new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 3, 5), new DateOnly(2023, 3, 10), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 600m, new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e") },
                    { new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 4, 15), new DateOnly(2023, 4, 20), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 700m, new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e") },
                    { new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 5, 8), new DateOnly(2023, 5, 15), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 550m, new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e") },
                    { new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"), new DateOnly(2023, 6, 20), new DateOnly(2023, 6, 25), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 800m, new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
