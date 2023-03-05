using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    public class CarryOutTradeCommand : AuthenticatedRouteCommand
    {
        private readonly ITradingManager _tradingManager;
        private readonly Guid tradeID;
        private readonly Guid cardID;
        public CarryOutTradeCommand(User identity, ITradingManager tradingManager, Guid tradeID, Guid cardID) : base(identity)
        {
            _tradingManager = tradingManager;
            this.tradeID = tradeID;
            this.cardID = cardID;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _tradingManager.AcceptTradingDeal(this.Identity.Credentials, cardID, tradeID);
                response.StatusCode = StatusCode.Ok;
            }
            catch (CardNotOwnedOrRequirementsNotMetException)
            {
                response.StatusCode = StatusCode.Forbidden;
            }
            catch (CardNotFoundException)
            {
                response.StatusCode = StatusCode.NotFound;
            }
            return response;
        }
    }
}
