using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    [Serializable]
    internal class NotEnoughCardsinDeckException : Exception
    {
        public NotEnoughCardsinDeckException()
        {
        }

        public NotEnoughCardsinDeckException(string? message) : base(message)
        {
        }

        public NotEnoughCardsinDeckException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotEnoughCardsinDeckException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}