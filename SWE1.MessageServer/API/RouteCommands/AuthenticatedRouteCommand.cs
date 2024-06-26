﻿using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands
{
    public abstract class AuthenticatedRouteCommand : IRouteCommand
    {
        public User Identity { get; private set; }

        public AuthenticatedRouteCommand(User identity)
        {
            Identity = identity;
        }

        public abstract Response Execute();
    }
}
