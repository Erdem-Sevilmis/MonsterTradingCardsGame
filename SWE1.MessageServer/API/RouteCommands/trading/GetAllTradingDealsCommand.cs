using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    public class GetAllTradingDealsCommand : AuthenticatedRouteCommand
    {

        private readonly ITradingManager _tradingManager;

        public GetAllTradingDealsCommand(User identity, ITradingManager tradingManager) : base(identity)
        {
            _tradingManager = tradingManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                var tradingDeals = _tradingManager.GetTradingDeals(this.Identity.Credentials);
                response.Payload = JsonConvert.SerializeObject(tradingDeals.Select(deal => new { Id = deal.Id, CardToTrade = deal.CardToTrade, Type = deal.Type, MinimumDamage = deal.MinimumDamage }).ToArray());
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
