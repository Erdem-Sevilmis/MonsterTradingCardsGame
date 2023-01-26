using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.trading
{
    internal interface ITradingManager
    {
        bool CreateNewTradingDeal(Credentials credentials, TradingDeal tradingdeal);
        List<Guid> GetTradingDeals(Credentials credentials);
        bool DeleteTradingDeal(Credentials credentials, TradingDeal tradingdeal);
        bool AcceptTradingDeal(Credentials credentials, Guid cardId, TradingDeal tradingDeal);
    }
}
