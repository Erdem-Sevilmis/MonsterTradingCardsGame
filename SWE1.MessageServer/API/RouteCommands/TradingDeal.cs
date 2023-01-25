using MonsterTradingCardsGame;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal class TradingDeal
    {
        public Guid CardToTrade { get; set; }
        public DataBase.Card_Type Type { get; set; }
        public int MinimumDamage { get; set; }

        public TradingDeal(Guid cardToTrade, DataBase.Card_Type type, int minimumDamage)
        {
            CardToTrade = cardToTrade;
            Type = type;
            MinimumDamage = minimumDamage;
        }
    }
}