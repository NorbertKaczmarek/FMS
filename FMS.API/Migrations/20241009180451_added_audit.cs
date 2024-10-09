using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.API.Migrations
{
    /// <inheritdoc />
    public partial class added_audit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Users",
                type: "datetime",
                nullable: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Users",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Flights",
                type: "datetime",
                nullable: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Flights",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Flights");
        }
    }
}
