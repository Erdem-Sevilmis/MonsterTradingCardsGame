using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Test
{
    internal class TradingTest
    {
        TradingManager tradingManager;
        DataBaseTradingDao tradingDao;

        [SetUp]
        public void SetUp()
        {
            tradingDao = new DataBaseTradingDao();
            tradingManager = new TradingManager();
        }
    }
}
