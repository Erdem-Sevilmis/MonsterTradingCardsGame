namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.User
{
    public class UserStats
    {
       
        public string? Name { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        
        public UserStats(string? name, int elo, int wins, int losses)
        {
            this.Name = name;
            this.Elo = elo;
            this.Wins = wins;
            this.Losses = losses;
        }

        public override string? ToString()
        {
            return $"\tName: {Name}\n\tElo: {Elo}\n\tWins: {Wins}\n\tLosses: {Losses}";
        }
    }
}