namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.User
{
    public class UserData
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public UserData(string name, string bio, string image)
        {
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}