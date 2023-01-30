using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    internal class GetAllTradingDealsCommand : NewPackageCommand
    {
        
        private readonly ITradingManager _tradingManager;

        public GetAllTradingDealsCommand(User identity, ITradingManager tradingManager): base(identity)
        {
            _tradingManager = tradingManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _tradingManager.GetTradingDeals(this.Identity.Credentials);
                response.StatusCode = StatusCode.Ok;
            }
            catch (NoTradsAvailbleException)
            {
                response.StatusCode = StatusCode.NoContent;
            }
            return response;
        }
    }
}
