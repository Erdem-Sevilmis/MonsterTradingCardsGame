using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.cards
{
    internal interface ICardsManager
    {
        List<Guid> GetUserCards(User identity);
        List<Guid> GetUserDeck(User identity);
        bool ConfigureNewDeck(User identity, Guid[] cardIds);
    }
}
