using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.API.RouteCommands.cards;
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
    internal class BuyPackageCommand : AuthenticatedRouteCommand
    {
        private readonly IPackageManager _packageManager;
        private readonly User identity;

        public BuyPackageCommand(User identity, IPackageManager packageManager) : base(identity)
        {
            _packageManager = packageManager;
            this.identity = identity;
        }

        public override Response Execute()
        {
            var response = new Response();
            if (identity.Coins < 5)
            {
                response.StatusCode = StatusCode.Forbidden;
                return response;
            }

            try
            {
                _packageManager.AcquireNewPackage(Identity);
                response.StatusCode = StatusCode.Ok;
            }
            catch (NoPackageAvailableException)
            {
                response.StatusCode = StatusCode.NotFound;
            }
            return response;
        }
    }
}
