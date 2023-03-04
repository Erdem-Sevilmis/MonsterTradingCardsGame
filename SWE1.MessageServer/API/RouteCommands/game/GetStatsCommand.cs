using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.Core.Response;
using System.Security.Principal;

namespace SWE1.MessageServer.API.RouteCommands.game
{
    public class GetStatsCommand : AuthenticatedRouteCommand
    {
        private User identity;
        private IGameManager gameManager;

        public GetStatsCommand(User identity, IGameManager gameManager) : base(identity)
        {
            this.identity = identity;
            this.gameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                UserStats stats = gameManager.GetStats(this.Identity);
                response.Payload = JsonConvert.SerializeObject(stats);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                //Nothing in table stats 
                response.StatusCode = StatusCode.InternalServerError;
            }
            return response;
        }
    }
}