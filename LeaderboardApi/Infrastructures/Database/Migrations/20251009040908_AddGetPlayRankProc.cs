using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGetPlayRankProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            CREATE PROCEDURE GetPlayerRank
                @PlayerId INT
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT CAST(Rank AS int) AS Value
                FROM (
                    SELECT Id, ROW_NUMBER() OVER (ORDER BY Score DESC) AS Rank
                    FROM Players
                ) AS Ranked
                WHERE Id = @PlayerId;
            END";
        
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPlayerRank");
        }
    }
}
