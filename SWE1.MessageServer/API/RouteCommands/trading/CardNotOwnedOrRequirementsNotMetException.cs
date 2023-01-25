using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    [Serializable]
    internal class CardNotOwnedOrRequirementsNotMetException : Exception
    {
        public CardNotOwnedOrRequirementsNotMetException()
        {
        }

        public CardNotOwnedOrRequirementsNotMetException(string? message) : base(message)
        {
        }

        public CardNotOwnedOrRequirementsNotMetException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CardNotOwnedOrRequirementsNotMetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}