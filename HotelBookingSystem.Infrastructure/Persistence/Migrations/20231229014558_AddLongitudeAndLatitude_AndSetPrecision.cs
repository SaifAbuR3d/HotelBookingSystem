using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLongitudeAndLatitude_AndSetPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Hotels",
                type: "float(8)",
                precision: 8,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Hotels",
                type: "float(9)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 12.345677999999999, 153.45678899999999 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 13.345677999999999, 143.45678899999999 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 14.345677999999999, 133.45678899999999 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 15.345677999999999, 123.456789 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 16.345677999999999, 113.456789 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Hotels");
        }
    }
}
