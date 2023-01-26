using static MonsterTradingCardsGame.DataBase;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class Card
    {
        public Card_Type Name { get; set; }
        public double Damage { get; set; }
        public Guid Id { get; set; }
        public ElementType ElementType { get; set; }

        public Card(Card_Type name, double damage, Guid card_Id, ElementType elementType)
        {
            Name = name;
            Damage = damage;
            Id = card_Id;
            ElementType = elementType;
        }

        public override string? ToString()
        {
            return $"{Name} {Damage} {ElementType}";
        }
    }
}