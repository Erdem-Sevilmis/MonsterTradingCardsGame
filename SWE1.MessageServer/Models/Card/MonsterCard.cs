using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.DataBase;

namespace MonsterTradingCardsGame.SWE1.MessageServer.Models.Card
{
    public class MonsterCard:Card
    {
        public MonsterType MonsterType { get; set; }
        public MonsterCard(Card_Type name, double damage, ElementType elementType, MonsterType monsterType,Guid card_id) : base(name, damage, card_id)
        {
            this.MonsterType = monsterType;
        }        
    }
}
