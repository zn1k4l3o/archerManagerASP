using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcherManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompetitionArcherMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Competitions_CompetitionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompetitionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "Users");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompetitionId",
                table: "Users",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Competitions_CompetitionId",
                table: "Users",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");
        }
    }
}
