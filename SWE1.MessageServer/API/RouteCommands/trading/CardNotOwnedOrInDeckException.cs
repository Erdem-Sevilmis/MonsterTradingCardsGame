using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    [Serializable]
    internal class CardNotOwnedOrInDeckException : Exception
    {
        public CardNotOwnedOrInDeckException()
        {
        }

        public CardNotOwnedOrInDeckException(string? message) : base(message)
        {
        }

        public CardNotOwnedOrInDeckException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CardNotOwnedOrInDeckException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}