using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    [Serializable]
    public class DeckEmptyException : Exception
    {
        public DeckEmptyException()
        {
        }

        public DeckEmptyException(string? message) : base(message)
        {
        }

        public DeckEmptyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DeckEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}