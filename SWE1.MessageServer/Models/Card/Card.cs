using static MonsterTradingCardsGame.DataBase;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class Card
    {
        public Card_Type Name { get; set; }
        public double Damage { get; set; }
        public Guid Id { get; set; }
        public ElementType ElementType { get; set; }

        public Card(Card_Type name, double damage, Guid card_Id)
        {
            Name = name;
            Damage = damage;
            Id = card_Id;
            SetElementType();
        }


        private void SetElementType()
        {
            switch (Name.ToString())
            {
                case string name when name.StartsWith("Fire"):
                    ElementType = ElementType.Fire;
                    break;
                case string name when name.StartsWith("Water"):
                    ElementType = ElementType.Water;
                    break;
                case string name when name.StartsWith("Regular"):
                    ElementType = ElementType.Normal;
                    break;
                case string name when name.Equals("Knight"):
                    ElementType = ElementType.Normal;
                    break;
                case string name when name.Equals("Dragon"):
                    ElementType = ElementType.Fire;
                    break;
                case string name when name.Equals("Ork"):
                    ElementType = ElementType.Normal;
                    break;
                case string name when name.Equals("Kraken"):
                    ElementType = ElementType.Water;
                    break;
                case string name when name.Equals("Wizzard"):
                    ElementType = ElementType.Normal;
                    break;

                default:
                    break;
            }
        }

        public override string? ToString()
        {
            return $"{Name} {Damage} {ElementType}";
        }
    }
}