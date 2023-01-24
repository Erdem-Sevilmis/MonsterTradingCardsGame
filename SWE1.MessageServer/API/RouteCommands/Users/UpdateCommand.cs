using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.Users
{
    internal class UpdateCommand : IRouteCommand
    {
        private readonly Credentials _credentials;
        private readonly IUserManager _userManager;

        public UpdateCommand(Credentials credentials, IUserManager userManager)
        {
            _credentials = credentials;
            _userManager = userManager;
        }

        public Response Execute()
        {
            throw new NotImplementedException();
        }
    }
}
