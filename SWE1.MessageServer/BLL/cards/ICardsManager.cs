using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.cards
{
    internal interface ICardsManager
    {
        List<Guid> GetUserCards(string username);
        List<Guid> GetUserDeck(string username);
        bool ConfigureNewDeck(string username, Guid[] cardIds);
    }
}
