using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Core.Response;

namespace SWE1.MessageServer.API.RouteCommands.Messages
{
    internal class UpdateMessageCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly string _content;
        private readonly int _messageId;

        public UpdateMessageCommand(IMessageManager messageManager, User identity, int messageId, string content) : base(identity)
        {
            _messageId = messageId;
            _content = content;
            _messageManager = messageManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _messageManager.UpdateMessage(Identity, _messageId, _content);
                response.StatusCode = StatusCode.Ok;
            }
            catch (MessageNotFoundException)
            {
                response.StatusCode = StatusCode.NotFound;
            }

            return response;
        }
    }
}
