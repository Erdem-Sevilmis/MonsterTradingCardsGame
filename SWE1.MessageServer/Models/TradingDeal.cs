using MonsterTradingCardsGame;

namespace SWE1.MessageServer.Models
{
    public partial class TradingDeal
    {
        public Guid Id { get; set; }
        public Guid CardToTrade { get; set; }
        public SpellMonster Type { get; set; }
        public int MinimumDamage { get; set; }

        public TradingDeal(Guid id, Guid cardToTrade, SpellMonster type, int minimumDamage)
        {
            Id = id;
            CardToTrade = cardToTrade;
            this.Type = type;
            MinimumDamage = minimumDamage;
        }
    }
}