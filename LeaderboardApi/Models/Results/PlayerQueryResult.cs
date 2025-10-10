namespace LeaderboardApi.Models.Results
{
    public class PlayerQueryResult
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime LastSubmitDate { get; set; }
        public Int64 Rank { get; set; }
    }
}