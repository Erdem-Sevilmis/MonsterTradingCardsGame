using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.Users
{
    internal class GetCommand : AuthenticatedRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly string username;
        private readonly User identity;

        public GetCommand(User identity, IUserManager userManager, string username) : base(identity)
        {
            _userManager = userManager;
            this.username = username;
            this.identity = identity;
        }

        public override Response Execute()
        {
            var response = new Response();
            if (identity.Token != username + "-mtcgToken" && username != "admin")
            {
                response.StatusCode = StatusCode.Unauthorized;
                return response;
            } 
            User user;

            user = _userManager.GetUserByAuthToken(identity.Token);
            if (user == null)
            {
                response.StatusCode = StatusCode.NotFound;
                return response;
            }
            var userdata = _userManager.GetUserData(user);
            string message = String.Empty;
            if (userdata.IsEmpty())
            {
                message = "\t" + "UserData is empty." + "\n";
                response.Payload = message;
                response.StatusCode = StatusCode.NoContent;
                return response;

            }
            message = userdata.ToString() + "\n";
            response.Payload = message;
            response.StatusCode = StatusCode.Ok;
            return response;
        }
    }
}
