using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ourTube.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReasonToBannedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "BannedUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "BannedUsers");
        }
    }
}
