using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.API.RouteCommands.trading;
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
    internal class GetDeckCommand : AuthenticatedRouteCommand
    {
        private readonly ICardsManager _cardsManager;
        public GetDeckCommand(User identity, ICardsManager cardsManager): base(identity)
        {
            _cardsManager = cardsManager;
        }
        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _cardsManager.GetUserDeck(this.Identity);
                response.StatusCode = StatusCode.Ok;
            }
            catch (NotEnoughCardsinDeckException)
            {
                response.StatusCode = StatusCode.BadRequest;
            }
            catch (DeckEmptyException)
            {
                response.StatusCode = StatusCode.NoContent;
            }
            return response;
        }
    }
}
