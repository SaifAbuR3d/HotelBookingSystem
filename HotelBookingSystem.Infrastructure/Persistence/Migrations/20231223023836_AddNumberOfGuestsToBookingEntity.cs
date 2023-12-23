using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfGuestsToBookingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfAdults",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] {1, 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] {1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAdults",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "Bookings");
        }
    }
}
