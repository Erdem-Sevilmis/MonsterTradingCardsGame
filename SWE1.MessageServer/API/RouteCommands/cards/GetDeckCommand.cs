using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
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
        private readonly string format;


        public GetDeckCommand(User identity, ICardsManager cardsManager, string format) : base(identity)
        {
            _cardsManager = cardsManager;
            this.format = format;
        }
        public override Response Execute()
        {
            var response = new Response();
            if (!(string.Equals("json", format) || string.Equals("plain", format)))
            {
                response.StatusCode = StatusCode.BadRequest;
                return response;
            }

            List<Card> cards;
            try
            {
                cards = _cardsManager.GetUserDeck(this.Identity);
                if (cards.Count > 0)
                {
                    response.StatusCode = StatusCode.Ok;
                    switch (format)
                    {
                        case "json":
                            response.Payload = JsonConvert.SerializeObject(cards.Select(card => new { id = card.Id, name = card.Name, damage = card.Damage, element = card.ElementType }).ToArray());
                            break;
                        case "plain":
                            string message = "Your deck includes:\n";
                            foreach (var card in cards)
                            {
                                message += "\t" + card.ToString() + "\n";
                            }
                            response.Payload = message;
                            break;
                    }
                }
                else
                {
                    response.StatusCode = StatusCode.NoContent;
                }
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
