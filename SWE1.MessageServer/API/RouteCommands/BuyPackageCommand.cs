using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.Core.Response;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal class BuyPackageCommand : AuthenticatedRouteCommand
    {
        private User user;
        private IPackageManager packageManager;

        public BuyPackageCommand(User user, IPackageManager packageManager): base(user)
        {
            this.user = user;
            this.packageManager = packageManager;
        }

        public override Response Execute()
        {
            throw new NotImplementedException();
        }
    }
}