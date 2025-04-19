using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Dal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewToRoomTypeBased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Rooms_RoomId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RoomId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "RoomType",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RoomId",
                table: "Reviews",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Rooms_RoomId",
                table: "Reviews",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
