using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    [Serializable]
    internal class NoCardsException : Exception
    {
        public NoCardsException()
        {
        }

        public NoCardsException(string? message) : base(message)
        {
        }

        public NoCardsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoCardsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}