using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    [Serializable]
    public class CardNotOwnedOrUnavailableException : Exception
    {
        public CardNotOwnedOrUnavailableException()
        {
        }

        public CardNotOwnedOrUnavailableException(string? message) : base(message)
        {
        }

        public CardNotOwnedOrUnavailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CardNotOwnedOrUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}