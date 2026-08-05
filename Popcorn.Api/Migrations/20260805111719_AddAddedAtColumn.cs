using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Popcorn.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAddedAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "AddedAt",
                table: "Activities",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Activities");
        }
    }
}
