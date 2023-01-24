using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.trading
{
    internal class TradingManager : ITradingManager
    {
        public bool AcceptTradingDeal(string username, Guid cardId)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTradingDeal(string username, Guid cardId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTradingDeal(string username, Guid cardId)
        {
            throw new NotImplementedException();
        }

        public List<Guid> GetTradingDeals(string username)
        {
            throw new NotImplementedException();
        }
    }
}
