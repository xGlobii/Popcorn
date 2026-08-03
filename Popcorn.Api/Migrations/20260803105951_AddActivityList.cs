using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Popcorn.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaItem",
                columns: table => new
                {
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItem", x => new { x.MediaType, x.TmdbId });
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => new { x.MediaType, x.TmdbId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Activities_MediaItem_MediaType_TmdbId",
                        columns: x => new { x.MediaType, x.TmdbId },
                        principalTable: "MediaItem",
                        principalColumns: new[] { "MediaType", "TmdbId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "MediaItem");
        }
    }
}
