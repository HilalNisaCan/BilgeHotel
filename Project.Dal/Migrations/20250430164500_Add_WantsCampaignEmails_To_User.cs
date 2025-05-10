using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Add_WantsCampaignEmails_To_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WantsCampaignEmails",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WantsCampaignEmails",
                table: "AspNetUsers");
        }
    }
}
