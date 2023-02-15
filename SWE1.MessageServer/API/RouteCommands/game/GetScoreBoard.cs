using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
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
                string message = String.Empty;
                for (int i = 0; i < stats.Count; i++)
                {
                    message += (i + 1) + ":\n" + stats[i].ToString() + "\n";
                }
                response.Payload = message;
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