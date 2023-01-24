using System.Net;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.User
{
    public class User
    {
        public int Coins { get; set; }
        public string Token { get; set; }
        public Credentials Credentials { get; set; }

        public List<Card.Card> Deck = new();
        private List<Card.Card> Stack = new();

        public User(string username, string password)
        {
            Credentials = new Credentials(username, password);
        }
        
        public bool BuyPackage()
        {
            return false;
        }
        
        public bool Battle()
        {
            return false;
        }
    }
}
