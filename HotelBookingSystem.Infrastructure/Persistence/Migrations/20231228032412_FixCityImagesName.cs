using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCityImagesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityImage_Cities_CityId",
                table: "CityImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityImage",
                table: "CityImage");

            migrationBuilder.RenameTable(
                name: "CityImage",
                newName: "CityImages");

            migrationBuilder.RenameIndex(
                name: "IX_CityImage_CityId",
                table: "CityImages",
                newName: "IX_CityImages_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityImages",
                table: "CityImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityImages_Cities_CityId",
                table: "CityImages",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityImages_Cities_CityId",
                table: "CityImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityImages",
                table: "CityImages");

            migrationBuilder.RenameTable(
                name: "CityImages",
                newName: "CityImage");

            migrationBuilder.RenameIndex(
                name: "IX_CityImages_CityId",
                table: "CityImage",
                newName: "IX_CityImage_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityImage",
                table: "CityImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityImage_Cities_CityId",
                table: "CityImage",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
