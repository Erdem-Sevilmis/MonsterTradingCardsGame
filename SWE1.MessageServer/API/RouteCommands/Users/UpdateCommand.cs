﻿using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.Users
{
    internal class UpdateCommand : AuthenticatedRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly string username;
        private readonly User identity;

        public UpdateCommand(User identity, IUserManager userManager, string username):base(identity)
        {
            _userManager = userManager;
            this.username = username;
            this.identity = identity;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _userManager.UpdateUser(identity, username);
                response.StatusCode = StatusCode.Ok;
            }
            catch (UserNotFoundException)
            {
                response.StatusCode = StatusCode.NotFound;
            }
            return response;
        }
    }
}
