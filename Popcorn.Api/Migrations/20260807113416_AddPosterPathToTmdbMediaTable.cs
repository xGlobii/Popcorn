using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Popcorn.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPosterPathToTmdbMediaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "MediaItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "MediaItem");
        }
    }
}
