using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.Core.Response;

namespace SWE1.MessageServer.API.RouteCommands.game
{
    internal class GetScoreBoard : AuthenticatedRouteCommand
    {
        private User identity;
        private IGameManager gameManager;

        public GetScoreBoard(User identity, IGameManager gameManager) : base(identity)
        {
            this.identity = identity;
            this.gameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                var stats = gameManager.GetScoreboard(this.Identity);
                response.Payload = JsonConvert.SerializeObject(stats.Select(stat => new { Name = stat.Name, Elo = stat.Elo, Winns = stat.Wins, Losses = stat.Losses }).ToArray());
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                response.StatusCode = StatusCode.InternalServerError;
            }
            return response;
        }
    }
}