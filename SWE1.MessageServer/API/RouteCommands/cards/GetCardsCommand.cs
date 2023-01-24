using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
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
    internal class GetCardsCommand : IRouteCommand
    {
        private readonly Credentials _credentials;
        private readonly ICardsManager _cardsManager;

        public GetCardsCommand(Credentials credentials, ICardsManager cardsManager)
        {
            _credentials = credentials;
            _cardsManager = cardsManager;
        }

        public Response Execute()
        {
            throw new NotImplementedException();
        }
    }
}
