using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGetRelativePlayersProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            CREATE PROCEDURE GetRelativePlayers
                @PlayerId INT,
                @Range INT
            AS
            BEGIN
                SET NOCOUNT ON;

                WITH Ranked AS (
                    SELECT 
                        Id,
                        Score,
                        LastSubmitDate,
                        ROW_NUMBER() OVER (ORDER BY Score DESC, LastSubmitDate ASC) AS Rank
                    FROM Players
                )
                SELECT 
                    r.Id,
                    r.Score,
                    r.LastSubmitDate,
                    r.Rank
                FROM Ranked r
                CROSS JOIN (
                    SELECT Rank AS PlayerRank 
                    FROM Ranked 
                    WHERE Id = @PlayerId
                ) pr
                WHERE r.Rank BETWEEN pr.PlayerRank - @Range AND pr.PlayerRank + @Range
                ORDER BY r.Rank;
            END";
            
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetRelativePlayers");
        }
    }
}
