using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.packages
{
    internal class NewPackageCommand : AuthenticatedRouteCommand
    {
        private readonly IPackageManager _packageManager;
        private readonly Card[] cardIds;
        private readonly User identity;

        public NewPackageCommand(User identity, IPackageManager packageManager, Card[] cardIds) : base(identity)
        {
            _packageManager = packageManager;
            this.cardIds = cardIds;
            this.identity = identity;
        }

        public override Response Execute()
        {
            var response = new Response();
            
            if (identity.Credentials.Username != "admin")
            {
                response.StatusCode = StatusCode.Forbidden;
                return response;
            }
            
            try
            {
                _packageManager.NewPackage(this.Identity, cardIds);
                response.StatusCode = StatusCode.Created;
            }
            catch (AtLeastOneCardInThePackageAlreadyExistsException)
            {
                response.StatusCode = StatusCode.Conflict;
            }
            return response;
        }
    }
}
