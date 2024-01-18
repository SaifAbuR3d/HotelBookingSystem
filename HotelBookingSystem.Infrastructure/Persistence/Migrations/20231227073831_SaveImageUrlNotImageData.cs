using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SaveImageUrlNotImageData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "RoomImages");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "HotelImages");


            migrationBuilder.AlterColumn<string>(
                name: "ImageData",
                table: "RoomImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.RenameColumn(
                name: "ImageData",
                table: "RoomImages",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "AlternativeText",
                table: "RoomImages",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.AlterColumn<string>(
                name: "ImageData",
                table: "HotelImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.RenameColumn(
                name: "ImageData",
                table: "HotelImages",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "AlternativeText",
                table: "HotelImages",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.CreateTable(
                name: "CityImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityImage_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityImage_CityId",
                table: "CityImage",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityImage");

            migrationBuilder.DropColumn(
                name: "AlternativeText",
                table: "RoomImages");

            migrationBuilder.DropColumn(
                name: "AlternativeText",
                table: "HotelImages");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageUrl",
                table: "RoomImages",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "RoomImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageUrl",
                table: "HotelImages",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "HotelImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
