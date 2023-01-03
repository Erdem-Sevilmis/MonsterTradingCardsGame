namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class Card
    {
        public string Name { get; set; }
        public double Damage { get; set; }
        public ElementType ElementType { get; set; }

        public Card(string name, double damage, ElementType elementType)
        {
            Name = name;
            Damage = damage;
            ElementType = elementType;
        }
    }
}