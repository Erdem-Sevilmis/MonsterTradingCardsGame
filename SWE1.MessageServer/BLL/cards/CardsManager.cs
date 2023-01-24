using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.cards
{
    internal class CardsManager : ICardsManager
    {
        public bool ConfigureNewDeck(string username, Guid[] cardIds)
        {
            throw new NotImplementedException();
        }

        public List<Guid> GetUserCards(string username)
        {
            throw new NotImplementedException();
        }

        public List<Guid> GetUserDeck(string username)
        {
            throw new NotImplementedException();
        }
    }
}
