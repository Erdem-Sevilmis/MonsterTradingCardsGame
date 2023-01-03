using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Core.Response;

namespace SWE1.MessageServer.API.RouteCommands.Messages
{
    internal class AddMessageCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly string _message;

        public AddMessageCommand(IMessageManager messageManager, User identity, string message) : base(identity)
        {
            _messageManager = messageManager;
            _message = message;
        }

        public override Response Execute()
        {
            var message = _messageManager.AddMessage(Identity, _message);

            var response = new Response()
            {
                StatusCode = StatusCode.Created,
                Payload = message.Id.ToString()
            };

            return response; 
        }
    }
}
