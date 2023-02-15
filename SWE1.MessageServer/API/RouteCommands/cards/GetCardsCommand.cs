﻿using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    internal class GetCardsCommand : AuthenticatedRouteCommand
    {
        private readonly ICardsManager _cardsManager;

        public GetCardsCommand(User identity, ICardsManager cardsManager) : base(identity)
        {
            _cardsManager = cardsManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                var userCards = _cardsManager.GetUserCards(this.Identity);
                response.StatusCode = StatusCode.Ok;
                string message = String.Empty;
                foreach (var card in userCards)
                {
                    message += "\t" + card.ToString() + "\n";
                }
                response.Payload = message;
            }
            catch (NoCardsException)
            {
                response.StatusCode = StatusCode.NoContent;
            }
            return response;
        }
    }
}
