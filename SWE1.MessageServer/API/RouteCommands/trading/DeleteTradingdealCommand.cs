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
    public class DeleteTradingdealCommand : AuthenticatedRouteCommand
    {
        private readonly ITradingManager _tradingManager;
        private readonly Guid cardid;
        public DeleteTradingdealCommand(User identity, ITradingManager tradingManager, Guid cardid) : base(identity)
        {
            _tradingManager = tradingManager;
            this.cardid = cardid;
        }
        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _tradingManager.DeleteTradingDeal(this.Identity.Credentials, cardid);
                response.StatusCode = StatusCode.Ok;
            }
            catch (CardNotOwnedOrInDeckException)
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
