using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.Core.Response;
using System.Security.Principal;

namespace SWE1.MessageServer.API.RouteCommands.game
{
    internal class GetStatsCommand : AuthenticatedRouteCommand
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
                var stats = gameManager.GetStats(this.Identity);
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