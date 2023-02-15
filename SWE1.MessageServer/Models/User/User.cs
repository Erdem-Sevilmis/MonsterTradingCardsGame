using System.Net;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.User
{
    public class User
    {
        public int Coins { get; set; }
        public string Token => $"{Credentials.Username}-mtcgToken";
        public Credentials Credentials { get; set; }
        public UserData UserData { get; set; }

        public List<Guid> Deck = new();

        private List<Guid> Stack = new();


        public User(string username, string password, int coins)
        {
            Credentials = new Credentials(username, password);
            Coins = coins;

        }
        public override string? ToString()
        {
            return UserData.ToString();
        }
    }
}
