using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    LastSubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "LastSubmitDate", "Score" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1200 },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 900 },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1200 },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1450 },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1500 },
                    { 6, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1700 },
                    { 7, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 450 },
                    { 8, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 980 },
                    { 9, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1010 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Score",
                table: "Players",
                column: "Score",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
