using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.Core.Response;

namespace SWE1.MessageServer.API.RouteCommands.game
{
    internal class BattleCommand : AuthenticatedRouteCommand
    {
        private User user;
        private IGameManager gameManager;

        public BattleCommand(User user, IGameManager gameManager) : base(user)
        {
            this.user = user;
            this.gameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            response.StatusCode = StatusCode.NotImplemented;
            return response;
        }
    }
}