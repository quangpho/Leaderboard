namespace Model
{
    public class PlayerDto
    {
        public string PlayerId { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public DateTime LastSubmitDate { get; set; }
    }
}