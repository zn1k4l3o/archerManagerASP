using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcherManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompetitionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitionType",
                table: "Competitions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionType",
                table: "Competitions");
        }
    }
}
