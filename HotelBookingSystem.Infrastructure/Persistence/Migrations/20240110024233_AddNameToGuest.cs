using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "John", "Doe" });

            migrationBuilder.UpdateData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Jane", "Smith" });

            migrationBuilder.UpdateData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Alice", "Jones" });

            migrationBuilder.UpdateData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Bob", "Smith" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Guests");
        }
    }
}
