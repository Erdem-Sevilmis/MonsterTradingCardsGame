using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
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
    internal class CreateTradingDealCommand : AuthenticatedRouteCommand
    {
        private readonly ITradingManager _tradingManager;
        private TradingDeal tradingDeal;
        public CreateTradingDealCommand(User identity, ITradingManager tradingManager, TradingDeal tradingDeal) :base(identity)
        {
            _tradingManager = tradingManager;
            this.tradingDeal = tradingDeal;
        }
        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _tradingManager.CreateNewTradingDeal(this.Identity.Credentials,tradingDeal);
                response.StatusCode = StatusCode.Created;
            }
            catch (CardNotOwnedOrInDeckException)
            {
                response.StatusCode = StatusCode.Forbidden;
            }
            catch (CardDealAlredyExistsException)
            {
                response.StatusCode = StatusCode.Conflict;
            }
            return response;
        }
    }
}
