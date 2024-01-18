using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MultipleRoomsInOneBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
               table: "Bookings",
               keyColumn: "Id",
               keyValue: new Guid("1A6A0801-C782-4D51-9917-2D98AA9E52D9"));

            migrationBuilder.DeleteData(
               table: "Bookings",
               keyColumn: "Id",
               keyValue: new Guid("06C10DD0-4632-410A-816A-62284BF28472"));

            migrationBuilder.DeleteData(
               table: "Bookings",
               keyColumn: "Id",
               keyValue: new Guid("C4BA5670-84A1-4567-92D8-921120F26372"));

            migrationBuilder.DeleteData(
               table: "Bookings",
               keyColumn: "Id",
               keyValue: new Guid("98526F06-1678-43D2-B4DE-9EE5FA5371A6"));

            migrationBuilder.DeleteData(
               table: "Bookings",
               keyColumn: "Id",
               keyValue: new Guid("80D6BF2F-C88D-48F9-9BE8-9FFD5C232E97"));

            migrationBuilder.DeleteData(
               table: "Bookings",
               keyColumn: "Id",
               keyValue: new Guid("96674730-341D-4579-845F-C16F7C89DCB4"));

            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BookingRoom",
                columns: table => new
                {
                    BookingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoom", x => new { x.BookingsId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_BookingRoom_Bookings_BookingsId",
                        column: x => x.BookingsId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelId",
                table: "Bookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_RoomsId",
                table: "BookingRoom",
                column: "RoomsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "BookingRoom");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_HotelId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Bookings");
        }
    }
}
