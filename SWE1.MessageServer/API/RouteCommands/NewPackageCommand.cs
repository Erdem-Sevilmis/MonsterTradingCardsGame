using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.API.RouteCommands.Users;
using SWE1.MessageServer.BLL.package;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal class NewPackageCommand : LoginCommand
    {
        private User user;
        private IPackageManager packageManager;
        private Card[] cards;

        public NewPackageCommand(User user, IPackageManager packageManager, Card[] cards)
        {
            this.user = user;
            this.packageManager = packageManager;
            this.cards = cards;
        }
    }
}