using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Core.Response;

namespace SWE1.MessageServer.API.RouteCommands.Messages
{
    internal class ShowMessageCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly int _messageId;

        public ShowMessageCommand(IMessageManager messageManager, User identity, int messageId) : base(identity)
        {
            _messageId = messageId;
            _messageManager = messageManager;
        }

        public override Response Execute()
        {
            Message? message;
            try
            {
                message = _messageManager.ShowMessage(Identity, _messageId);
            }
            catch (MessageNotFoundException)
            {
                message = null;
            }

            var response = new Response();
            if (message == null)
            {
                response.StatusCode = StatusCode.NotFound;
            }
            else
            {
                response.Payload = message.Content;
                response.StatusCode = StatusCode.Ok;
            }

            return response;
        }
    }
}
