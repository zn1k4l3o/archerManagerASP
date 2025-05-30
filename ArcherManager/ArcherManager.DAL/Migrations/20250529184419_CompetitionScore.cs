using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcherManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompetitionScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Archers_Competitions_CompetitionId",
                table: "Archers");

            migrationBuilder.DropIndex(
                name: "IX_Archers_CompetitionId",
                table: "Archers");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "Archers");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "ArcherResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArcherResults_CompetitionId",
                table: "ArcherResults",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArcherResults_Competitions_CompetitionId",
                table: "ArcherResults",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArcherResults_Competitions_CompetitionId",
                table: "ArcherResults");

            migrationBuilder.DropIndex(
                name: "IX_ArcherResults_CompetitionId",
                table: "ArcherResults");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "ArcherResults");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "Archers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Archers_CompetitionId",
                table: "Archers",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Archers_Competitions_CompetitionId",
                table: "Archers",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");
        }
    }
}
