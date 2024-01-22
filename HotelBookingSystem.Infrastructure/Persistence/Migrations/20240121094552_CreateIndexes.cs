using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRoom_Bookings_BookingsId",
                table: "BookingRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRoom_Rooms_RoomsId",
                table: "BookingRoom");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Cities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IDX_Room_Type",
                table: "Rooms",
                column: "RoomType");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AdultsChildrenCapacity",
                table: "Rooms",
                columns: new[] { "AdultsCapacity", "ChildrenCapacity" });

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_StarRate",
                table: "Hotels",
                column: "StarRate");

            migrationBuilder.CreateIndex(
                name: "IDX_Discounts_StartDate_EndDate",
                table: "Discounts",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name_Country",
                table: "Cities",
                columns: new[] { "Name", "Country" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CheckInDate_CheckOutDate",
                table: "Bookings",
                columns: new[] { "CheckInDate", "CheckOutDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRoom_Bookings_BookingsId",
                table: "BookingRoom",
                column: "BookingsId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRoom_Rooms_RoomsId",
                table: "BookingRoom",
                column: "RoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRoom_Bookings_BookingsId",
                table: "BookingRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRoom_Rooms_RoomsId",
                table: "BookingRoom");

            migrationBuilder.DropIndex(
                name: "IDX_Room_Type",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_AdultsChildrenCapacity",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_StarRate",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IDX_Discounts_StartDate_EndDate",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name_Country",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CheckInDate_CheckOutDate",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRoom_Bookings_BookingsId",
                table: "BookingRoom",
                column: "BookingsId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRoom_Rooms_RoomsId",
                table: "BookingRoom",
                column: "RoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
