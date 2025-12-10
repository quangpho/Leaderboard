using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOnScoreAndPlayerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_Score",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Score_Id",
                table: "Players",
                columns: new[] { "Score", "Id" },
                descending: new[] { true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_Score_Id",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Score",
                table: "Players",
                column: "Score",
                descending: new bool[0]);
        }
    }
}
