using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.DataBase;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class SpellCard : Card
    {
        public new ElementType ElementType { get; set; }

        public SpellCard(Card_Type name, double damage, ElementType elementType, Guid card_id) : base(name, damage, card_id, elementType)
        {
            ElementType = elementType;
        }
    }
}
