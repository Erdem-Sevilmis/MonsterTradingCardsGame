using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
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
            List<string> log = gameManager.GetInToBattle(user);
            response.Payload = String.Join(" ", log);
            //Console.WriteLine(String.Join(" ", log));
            response.StatusCode = StatusCode.Ok;
            return response;
        }
    }
}