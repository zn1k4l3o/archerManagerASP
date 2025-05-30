using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcherManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompetitionDtartEnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CompetitionEnd",
                table: "Competitions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CompetitionStart",
                table: "Competitions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionEnd",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "CompetitionStart",
                table: "Competitions");
        }
    }
}
