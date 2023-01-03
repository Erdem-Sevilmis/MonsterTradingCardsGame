using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class SpellCard : Card
    {
        public new ElementType ElementType { get; set; }
        
        public SpellCard(string name, double damage, ElementType elementType) : base(name, damage, elementType)
        {
            ElementType = elementType;
        }
    }
}
