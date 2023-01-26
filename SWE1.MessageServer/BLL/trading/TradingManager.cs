using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.trading
{
    internal class TradingManager : ITradingManager
    {
        DataBaseTradingDao dataBaseTradingDao = new DataBaseTradingDao();
        public bool AcceptTradingDeal(Credentials credentials, Guid cardId, TradingDeal tradingDeal) => dataBaseTradingDao.Trade(credentials.Username, cardId, tradingDeal.CardToTrade);

        public bool CreateNewTradingDeal(Credentials credentials, TradingDeal tradingdeal) => dataBaseTradingDao.CreateNewTradingDeal(credentials.Username, tradingdeal.CardToTrade);

        public bool DeleteTradingDeal(Credentials credentials, TradingDeal tradingdeal) => dataBaseTradingDao.DeleteTradingDeal(credentials.Username, tradingdeal.CardToTrade);

        public List<Guid> GetTradingDeals(Credentials credentials) => dataBaseTradingDao.GetTradingDeals(credentials.Username);

    }
}
