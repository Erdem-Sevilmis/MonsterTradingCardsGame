using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class MonsterCard:Card
    {
        public MonsterType MonsterType { get; set; }
        public MonsterCard(string name, double damage, ElementType elementType, MonsterType monsterType) : base(name, damage, elementType)
        {
            this.MonsterType = monsterType;
        }        
    }
}
