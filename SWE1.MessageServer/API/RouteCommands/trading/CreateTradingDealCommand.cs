using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    internal class CreateTradingDealCommand : IRouteCommand
    {
        private readonly Credentials _credentials;
        private readonly ITradingManager _tradingManager;
        public CreateTradingDealCommand(Credentials credentials, ITradingManager tradingManager)
        {
            _credentials = credentials;
            _tradingManager = tradingManager;
        }
        public Response Execute()
        {
            throw new NotImplementedException();
        }
    }
}
