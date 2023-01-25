using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.API.RouteCommands.trading;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    internal class ConfigureDeckCommand : AuthenticatedRouteCommand
    {
        private readonly ICardsManager _cardsManager;
        private readonly Guid[] cardIds;

        public ConfigureDeckCommand(User identity, ICardsManager cardsManager, Guid[] cardIds) : base(identity)
        {
            _cardsManager = cardsManager;
            this.cardIds = cardIds;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _cardsManager.ConfigureNewDeck(this.Identity, cardIds);
                response.StatusCode = StatusCode.Ok;
            }
            catch (NotEnoughCardsinDeckException)
            {
                response.StatusCode = StatusCode.BadRequest;
            }
            catch (CardNotOwnedOrUnavailableException)
            {
                response.StatusCode = StatusCode.Forbidden;
            }
            return response;
        }
    }
}
