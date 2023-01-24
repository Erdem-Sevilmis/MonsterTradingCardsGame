using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.trading
{
    internal interface ITradingManager
    {
        bool CreateNewTradingDeal(string username, Guid cardId);
        List<Guid> GetTradingDeals(string username);
        bool DeleteTradingDeal(string username, Guid cardId);
        bool AcceptTradingDeal(string username, Guid cardId);
    }
}
