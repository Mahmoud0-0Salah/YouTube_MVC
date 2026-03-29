using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ourTube.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWatchVideoUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchVideos_VideoId_UserId",
                table: "WatchVideos");

            migrationBuilder.CreateIndex(
                name: "IX_WatchVideos_VideoId_UserId",
                table: "WatchVideos",
                columns: new[] { "VideoId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchVideos_VideoId_UserId",
                table: "WatchVideos");

            migrationBuilder.CreateIndex(
                name: "IX_WatchVideos_VideoId_UserId",
                table: "WatchVideos",
                columns: new[] { "VideoId", "UserId" },
                unique: true);
        }
    }
}
